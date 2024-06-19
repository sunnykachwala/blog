// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
"use strict";

// Preloader
$(window).on("load", function () {
    $(".loader").fadeOut("slow");
});

// Global
$(function () {

    //Dark/Light Mode
    $('#nightModeBtn').on('click', function (e) {
        let thememode = localStorage.getItem("thememode");
        if (thememode == 'dark') {
            localStorage.setItem("thememode", "light");
        } else {
            localStorage.setItem("thememode", "dark");
        }
        load_theme_setting();
    });

    var load_theme_setting = function () {
        let thememode = localStorage.getItem("thememode");
        if (thememode == 'dark') {
            $('body').addClass('dark');
            $('body').removeClass('light');
            $('#nightModeBtn').html('<i class="fa fa-lightbulb text-danger"></i>');
        }
        else {
            $('body').addClass('light');
            $('body').removeClass('dark');
            $('#nightModeBtn').html('<i class="fa fa-moon"></i>');
        }
    };
    load_theme_setting();

    /************************************
     *   Sidebar Layout
     ************************************/
    $('.sidebar-layout .menu-toggle').on('click', function (e) {
        w = $(window);
        if (w.outerWidth() <= 1024) {
            if ($('.sidebar-layout .app-menu').hasClass('show')) {
                $('.sidebar-layout .app-menu').removeClass('show');
            }
            else {
                $('.drawer-layout .app-menu').addClass('show');
            }
        } else {
            if ($('.sidebar-layout').hasClass('sidebar-mini')) {
                $('.sidebar-layout').removeClass('sidebar-mini');
                $(".sidebar-layout .app-menu .menu li.has-submenu").find("> .submenu .dropdown-title").remove();
                $('.sidebar-layout .app-menu li').find("> a").removeAttr('title');
                $('.sidebar-layout .app-menu li').find("> a").removeAttr('data-bs-toggle');
            }
            else {
                $('.sidebar-layout').addClass('sidebar-mini');
                $('.sidebar-layout .app-menu li').each(function () {
                    let me = $(this);
                    if (me.find("> .submenu").length) {
                        me.find("> .submenu").prepend(
                            '<li class="dropdown-title">' + me.find("> a").text() + "</li>"
                        );
                    } else {
                        me.find("> a").attr("data-bs-toggle", "tooltip");
                        me.find("> a").attr("title", me.find("> a").text());
                        $("[data-bs-toggle='tooltip']").tooltip({
                            placement: "right"
                        });
                    }
                });
            }
        }
    });

    $(".sidebar-layout .app-menu .menu li.has-submenu a")
        .on("click", function () {
            let isMini = $(".sidebar-layout").hasClass('sidebar-mini');
            if(!isMini){
                var me = $(this).parent().find("> .submenu");
                let selected = $(".sidebar-layout .app-menu .menu li.has-submenu").find("> .submenu.show");
                if (me.hasClass('show')) {
                    selected.removeClass('show');
                    selected.slideToggle(300, function () { return false; });
                    selected.parent().removeClass('toggled');
                }
                else {
                    selected.parent().removeClass('toggled');
                    selected.removeClass('show');
                    selected.slideToggle(300, function () { return false; });
                    me.slideToggle(300, function () { return false; });
                    me.addClass('show');
                    me.parent().addClass('toggled');
                }
            }
            
            return false;
        });

    //main-content minimum height
    $(".main-content").css({
        minHeight: $(window).outerHeight() - 95
    });

    var sidebar_dropdown = function () {
        if ($(".main-sidebar").length) {
            $(".main-sidebar .sidebar-menu li a.has-dropdown")
                .off("click")
                .on("click", function () {
                    var me = $(this);

                    me.parent()
                        .find("> .dropdown-menu")
                        .slideToggle(500, function () {
                            return false;
                        });
                    return false;
                });
        }
    };
    sidebar_dropdown();

    //toogle sidebar
    var toggle_sidebar_mini = function (mini) {
        let body = $("body");

        if (!mini) {
            body.removeClass("sidebar-mini");
            $(".main-sidebar .sidebar-menu > li > ul .dropdown-title").remove();
            $(".main-sidebar .sidebar-menu > li > a").removeAttr("title");
        } else {
            body.addClass("sidebar-mini");
            body.removeClass("sidebar-show");
            $(".main-sidebar .sidebar-menu > li").each(function () {
                let me = $(this);

                if (me.find("> .dropdown-menu").length) {
                    me.find("> .dropdown-menu").hide();
                    me.find("> .menu-toggle.toggled").toggleClass('toggled');
                    me.find("> .dropdown-menu").prepend(
                        '<li class="dropdown-title pt-3">' + me.find("> a").text() + "</li>"
                    );
                } else {
                    me.find("> a").attr("data-bs-toggle", "tooltip");
                    me.find("> a").attr("title", me.find("> a").text());
                    $("[data-bs-toggle='tooltip']").tooltip({
                        placement: "right"
                    });
                }
            });
        }
    };

    /************************************
     *  Drawer Layout
     ************************************/
    if ($('.drawer-layout').length) {
        $('.menu-toggle').on('click', function (e) {
            if ($('.drawer-layout .app-menu').hasClass('show')) {
                $('.drawer-layout .app-menu').removeClass('show');
            }
            else {
                $('.drawer-layout .app-menu').addClass('show');
            }
        });
        $('.drawer-layout .app-menu').before().on('click', function (e) {
            if ($('.drawer-layout .app-menu').hasClass('show')) {
                $('.drawer-layout .app-menu').removeClass('show');
            }
        });

        $(".drawer-layout .app-menu .menu li.has-submenu a")
            .on("click", function () {
                var me = $(this).parent().find("> .submenu");
                let selected = $(".drawer-layout .app-menu .menu li.has-submenu").find("> .submenu.show");
                if (me.hasClass('show')) {
                    selected.removeClass('show');
                    selected.slideToggle(300, function () { return false; });
                }
                else {
                    selected.removeClass('show');
                    selected.slideToggle(300, function () { return false; });
                    me.slideToggle(300, function () { return false; });
                    me.addClass('show');
                }

                return false;
            });
    }

    $("[data-toggle='sidebar']").click(function () {
        var body = $("body"),
            w = $(window);

        if (w.outerWidth() <= 1024) {
            if (body.hasClass("sidebar-gone")) {
                body.removeClass("sidebar-gone");
                body.addClass("sidebar-show");
            } else {
                body.addClass("sidebar-gone");
                body.removeClass("sidebar-show");
            }
        } else {
            if (body.hasClass("sidebar-mini")) {
                toggle_sidebar_mini(false);
            } else {
                toggle_sidebar_mini(true);
            }
        }

        return false;
    });

    // Background
    $("[data-background]").each(function () {
        var me = $(this);
        me.css({
            backgroundImage: "url(" + me.data("background") + ")"
        });
    });

    // Width attribute
    $("[data-width]").each(function () {
        $(this).css({
            width: $(this).data("width")
        });
    });

    // Height attribute
    $("[data-height]").each(function () {
        $(this).css({
            height: $(this).data("height")
        });
    });

    // full screen call
    $(document).on("click", "#fullScreenBtn", function (e) {
        if (
            !document.fullscreenElement && // alternative standard method
            !document.mozFullScreenElement &&
            !document.webkitFullscreenElement &&
            !document.msFullscreenElement
        ) {
            $('#fullScreenBtn').html('<i class="fa fa-compress"></i>');
            // current working methods
            if (document.documentElement.requestFullscreen) {
                document.documentElement.requestFullscreen();
            } else if (document.documentElement.msRequestFullscreen) {
                document.documentElement.msRequestFullscreen();
            } else if (document.documentElement.mozRequestFullScreen) {
                document.documentElement.mozRequestFullScreen();
            } else if (document.documentElement.webkitRequestFullscreen) {
                document.documentElement.webkitRequestFullscreen(
                    Element.ALLOW_KEYBOARD_INPUT
                );
            }
        } else {
            $('#fullScreenBtn').html('<i class="fa fa-expand"></i>');
            if (document.exitFullscreen) {
                document.exitFullscreen();
            } else if (document.msExitFullscreen) {
                document.msExitFullscreen();
            } else if (document.mozCancelFullScreen) {
                document.mozCancelFullScreen();
            } else if (document.webkitExitFullscreen) {
                document.webkitExitFullscreen();
            }
        }
    });
});
