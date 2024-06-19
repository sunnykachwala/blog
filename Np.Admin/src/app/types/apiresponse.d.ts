export interface APIResponse<T = any> {
  data: T;
  statusCode: number;
  message: string;
  errorMessage?: string;
}
