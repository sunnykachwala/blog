import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {

  private isLoading$$ = new BehaviorSubject<boolean>(false);
  isLoading$ = this.isLoading$$.asObservable();
  setLoading(isLoading: boolean) {
    this.isLoading$$.next(isLoading);
  }

  private headerName$$ = new BehaviorSubject<string>('');
  headerName$ = this.headerName$$.asObservable();

  setheaderName(headerName: string) {
    this.headerName$$.next(headerName);
  }
}
