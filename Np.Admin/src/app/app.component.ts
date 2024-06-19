import { HttpClient } from '@angular/common/http';
import { Component, OnInit, Renderer2 } from '@angular/core';
import { AppService } from './services/app.service';
//import * as $ from 'jquery';

declare var $: any;

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  public languageModal = {
    show: false,
    loading: false
  }
  public userDetails = {
    email: '',
    fullName: '',
    userName: '',
    language: 'en-US'
  }

  idleState = 'Not started.';
  timedOut = false;


  constructor(private http: HttpClient, public app: AppService) {
    $(window).on("load", function () {

      $(".splash").fadeOut("slow");
    });

  }

  public ngOnInit() {
   
    this.app.ApplyNightMode();

    $(function () {
      /************************************
       *   Sidebar Layout
       ************************************/
      if ($('.sidebar-layout').length) {
        //mobile menu backdrop click
        $('.sidebar-layout').on('click', function (e: any) {
          if ($(e.target).hasClass("sidebar-show")) {
            $(".sidebar-layout").removeClass("sidebar-show");
          }
        });

        //toogle sidebar
        var toggleSidebarMini = function (mini: boolean) {
          if (mini) {
            $('.sidebar-layout').addClass('sidebar-mini');
            hide_dropdown();
          }
          else {
            $('.sidebar-layout').removeClass('sidebar-mini');
            sidebar_dropdown();
          }
        };

        var sidebar_dropdown = function () {
          $(".sidebar-layout .app-menu .menu li.active").removeClass('active');
          $(".sidebar-layout .app-menu .menu li .submenu>li.active").removeClass('active');
          let selected = $(".sidebar-layout .app-menu .menu li a.active");
          if (selected.length) {
            selected.parent().addClass('active');
          }
          let selected1 = $(".sidebar-layout .app-menu .menu li .submenu>li a.active");
          if (selected1.length) {
            selected1.parent().addClass('active');
            selected1.parent().parent().parent().addClass('active');
            selected1.parent().parent().slideToggle(300, function () { return false; });
          }
        };

        var hide_dropdown = function () {
          let selected1 = $(".sidebar-layout .app-menu .menu li.has-submenu");
          if (selected1.length) {
            selected1.find('.submenu').removeAttr('style');
          }
        };

        //show/hide menu
        $('.menu-toggle').on('click', function (e: any) {
          let w = $(window);
          if (w.outerWidth() <= 1024) {
            if ($('.sidebar-layout').hasClass('sidebar-show')) {
              $('.sidebar-layout').removeClass('sidebar-show');
            }
            else {
              $('.sidebar-layout').addClass('sidebar-show');
            }
          } else {
            if ($('.sidebar-layout').hasClass('pos-page')) {
              return;
            }
            if ($('.sidebar-layout').hasClass('sidebar-mini')) {
              toggleSidebarMini(false);
            }
            else {
              toggleSidebarMini(true);
            }
          }
        });

        // Set active class on menu item and submenu item
        $('.app-menu .menu li a').on('click', function (e: any) {
          let isMini = $('.sidebar-layout').hasClass('sidebar-mini');

          let menuItem = $(e.currentTarget).parent();
          if (menuItem.hasClass('has-submenu')) {
            if (menuItem.hasClass('active')) {
              if (!isMini) {
                menuItem.find('> .submenu').slideToggle(300, function () { return false; });
              }
              menuItem.removeClass('active');
              return;
            }

            let selected = $(".app-menu .menu li.active");
            if (selected.length) {
              if (!$('.sidebar-layout').hasClass('sidebar-mini')) {
                if (!isMini) {
                  selected.find('> .submenu').slideToggle(300, function () { return false; });
                }
              }
              selected.removeClass('active');
            }

            if (!$('.sidebar-layout').hasClass('sidebar-mini')) {
              if (!isMini) {
                menuItem.find('> .submenu').slideToggle(300, function () { return false; });
              }
            }
            menuItem.addClass('active');
          }
          else {
            if (!menuItem.parent().hasClass('submenu')) {
              let selected = $(".app-menu .menu li.active");
              // Remove active class from all menu items
              if (!isMini) {
                selected.find('> .submenu').slideToggle(300, function () { return false; });
              }
              selected.removeClass('active');
            }
            else {
              $(".app-menu .menu li.has-submenu .submenu li.active").removeClass('active');
            }
            menuItem.addClass('active');

            let w = $(window);
            if (w.outerWidth() <= 1024) {
              if ($('.sidebar-layout').hasClass('sidebar-show')) {
                $('.sidebar-layout').removeClass('sidebar-show');
              }
              else {
                $('.sidebar-layout').addClass('sidebar-show');
              }
            }
          }
        });

        $(window).on("resize", function () {
        });
        toggleSidebarMini(false);
      }

      /************************************
       *   Drawer Layout
       ************************************/
      if ($('.drawer-layout').length) {
        //mobile menu backdrop click
        $('.drawer-layout').on('click', function (e: any) {
          if ($(e.target).hasClass("sidebar-show")) {
            $(".drawer-layout").removeClass("sidebar-show");
          }
        });

        //show/hide menu
        $('.menu-toggle').on('click', function (e: any) {
          if ($('.drawer-layout').hasClass('sidebar-show')) {
            $('.drawer-layout').removeClass('sidebar-show');
          }
          else {
            $('.drawer-layout').addClass('sidebar-show');
          }
        });

        var sidebar_dropdown = function () {
          $(".drawer-layout .app-menu .menu li.active").removeClass('active');
          $(".drawer-layout .app-menu .menu li .submenu>li.active").removeClass('active');
          let selected = $(".drawer-layout .app-menu .menu li a.active");
          if (selected.length) {
            selected.parent().addClass('active');
          }
          let selected1 = $(".drawer-layout .app-menu .menu li .submenu>li a.active");
          if (selected1.length) {
            selected1.parent().addClass('active');
            selected1.parent().parent().parent().addClass('active');
            selected1.parent().parent().slideToggle(300, function () { return false; });
          }
        };

        sidebar_dropdown();

        // Set active class on menu item and submenu item
        $('.app-menu .menu li a').on('click', function (e: any) {
          let menuItem = $(e.currentTarget).parent();
          if (menuItem.hasClass('has-submenu')) {
            if (menuItem.hasClass('active')) {
              menuItem.find('> .submenu').removeClass('show');
              menuItem.find('> .submenu').slideToggle(300, function () { return false; });
              menuItem.removeClass('active');
              return;
            }

            let selected = $(".app-menu .menu li.active");
            if (selected.length) {
              selected.find('> .submenu').removeClass('show');
              selected.find('> .submenu').slideToggle(300, function () { return false; });
              selected.removeClass('active');
            }

            menuItem.find('> .submenu').slideToggle(300, function () { return false; });
            menuItem.find('> .submenu').addClass('show');
            menuItem.addClass('active');
          }
          else {
            if (!menuItem.parent().hasClass('submenu')) {
              // Remove active class from all menu items
              $('.app-menu .menu li.active').removeClass('active');
            }
            else {
              $(".app-menu .menu li.has-submenu .submenu li.active").removeClass('active');
            }
            menuItem.addClass('active');

            if ($('.drawer-layout').hasClass('sidebar-show')) {
              $('.drawer-layout').removeClass('sidebar-show');
            }
            else {
              $('.drawer-layout').addClass('sidebar-show');
            }
          }
        });
      }

      // full screen call
      $(document).on("click", "#fullScreenBtn", function () {
        if (
          !document.fullscreenElement
        ) {
          $('#fullScreenBtn').html('<i class="fa fa-compress"></i>');
          // current working methods
          if (document.documentElement.requestFullscreen) {
            document.documentElement.requestFullscreen();
          }
        } else {
          $('#fullScreenBtn').html('<i class="fa fa-expand"></i>');
          if (document.exitFullscreen) {
            document.exitFullscreen();
          }
        }
      });

      $("[data-bs-toggle='tooltip']").tooltip();

      $(window).on("load", function () {
        $(".splash").fadeOut("slow");
      });
    });
  }

  public GetCountry(languageCode: string): string {
    const splitArray = languageCode?.split('-') || [];
    if (splitArray.length > 2) {
      return splitArray[2].toLocaleLowerCase();
    }
    else {
      return splitArray[1].toLocaleLowerCase();
    }
  }

  public ChangeLanguage(name: string) {
    this.app.ReloadLocalizationData(name);
    this.languageModal.show = false;
  }
}
