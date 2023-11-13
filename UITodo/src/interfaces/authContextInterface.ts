export interface IAuthProvider {
  children: React.ReactNode;
}

export interface AuthenticationContext {
  userId?: string;
  userName?: string;
  jwToken: string;
  jwtExpires: string;
  refreshToken: string;
  refreshTokenExpires: string;
  todoUser : number;
}
export interface AuthenticationContextType {
  isAuthenticated: boolean;
  login: ({
    userId,
    userName,
    jwToken,
    refreshToken,
    jwtExpires,
    refreshTokenExpires,
    todoUser
  }: AuthenticationContext) => void;
  logout: () => void;
  userName: string | null;
  todoUser: number | null
}
