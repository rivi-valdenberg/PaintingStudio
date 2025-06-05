export interface LoginRequest {
    username: string;
    password: string;
  }
  
  export interface LoginResponse {
    token: string;
  }
  
  export interface User {
    id: number;
    username: string;
    isAdmin: boolean;
  }