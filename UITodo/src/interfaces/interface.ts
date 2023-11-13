import Cookies from "universal-cookie";

export interface Register {
  name: string;
  lastName: string;
  email: string;
  username: string;
  password: string;
  confirmPassword: string;
}

export interface Login {
  email: string;
  password: string;
}

export interface ProblemDetails {
  title: string | undefined;
  status: number | undefined;
  detail: string | undefined;
}

export interface UserCookies {
  cookies: Cookies;
  userId?: string;
  userName?: string;
  jwToken: string;
  jwtExpires: string;
  refreshToken: string;
  refreshTokenExpires: string;
  todoUser: number;
}
export enum State {
  Active = "Activo",
  Completed = "Completado",
}

export interface AddTodo {
  userId?: number;
  description: string;
  dateLimit: string;
}

export interface TodoInformation {
  idTodo: number;
  description: string;
  dateLimit: string;
  estado: string;
}

export interface DeleteTodo {
  userId: number,
  todoId: number
}

export interface PutTodo {
  idTodo: number
  idUser: number
  estado: string
}


export enum filteredDataSetup
{
  All = "All",
  Active = "Activo",
  Completed = "Completado"
}