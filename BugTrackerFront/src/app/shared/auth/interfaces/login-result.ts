export interface LoginResult {
  success: boolean;
  message: string;
  token?: string;
  id?: string;
  userName?: string;
  roles?: Array<string>;
}
