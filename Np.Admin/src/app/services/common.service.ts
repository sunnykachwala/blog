import { Injectable } from '@angular/core';
//import { NzModalService } from '../ng-zorro/ng-zorro-antd.module/modal';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CommonService {
  timer = true;
  constructor(//private modal: NzModalService,
    private router: Router,
    private route: ActivatedRoute) { }

  openDialog(message: string) {
    //const dialogRef = this.modal.confirm({
    //  nzTitle: message,
    //  nzOkText: 'OK',
    //  nzCancelText: null,
    //  nzOnOk: () => {
    //    this.router.navigate(['/login']);
    //  }
    //});

    //dialogRef.afterOpen.subscribe(() => {
    //  console.log('Dialog opened');
    //  const dialogContainer = document.querySelector('.ant-modal-wrap');
    //  if (dialogContainer) {
    //    console.log('Container found');
    //    dialogContainer.classList.add('dark-background');
    //  }
    //  else {
    //    console.log('Container not found');
    //  }
    //});
  }

  openDialogWithOutRedirect(message: string) {
    //const dialogRef = this.modal.confirm({
    //  nzTitle: message,
    //  nzOkText: 'OK',
    //  nzCancelText: null,
    //  nzOnOk: () => {
    //    this.myTimer();
    //  }
    //});

    //dialogRef.afterOpen.subscribe(() => {
    //  console.log('Dialog opened');
    //  const dialogContainer = document.querySelector('.ant-modal-wrap');
    //  if (dialogContainer) {
    //    console.log('Container found');
    //    dialogContainer.classList.add('dark-background');
    //  }
    //  else {
    //    console.log('Container not found');
    //  }
    //});
  }

  myTimer() {
    setTimeout(() => {
      this.timer = true;
    }, 3000);
  }

  navigateToPage(route: string, queryParams?: any) {
    this.router.navigate([route], { queryParams: queryParams });
  }

  handleHttpError(error: HttpErrorResponse): string {
    let errorMessage = '';
    if (error.status === 400) {
      errorMessage = error.error.data.errorMessage || 'Bad Request';
    } else if (error.status === 401) {
      this.router.navigate(['/login']);
      errorMessage = 'Unauthorized Access.';
    } else if (error.status === 0) {
      // Set the error message if it's a 401 error
      errorMessage = 'The backend service is down. Please contact the system administrator.';
    } else if (error.status === 419) {
      this.router.navigate(['/login']);
      errorMessage = 'Token is expired. Please login again.';
    } else if (error.status === 500) {
      errorMessage = error.error.data.errorMessage;
    }
    else {
      console.log(error);
      errorMessage = 'Something goes wrong.';
    }
    return errorMessage;
  }
}
