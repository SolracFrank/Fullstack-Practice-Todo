import {
  DeleteTodo,
  PutTodo,
  TodoInformation,
} from "./../interfaces/interface";
import { AddTodo } from "../interfaces/interface.ts";
import api from "./axiosService.ts";
import axios from "axios";

export const addTodoService = async (TodoData: AddTodo) => {
  const response = await api.post(`/todo/${TodoData.userId}`, TodoData);
  return response;
};

export const getAllTodoService = async (idUser: number) => {
  try {
    const response = await api.get<TodoInformation[]>(`/todo/${idUser}/getall`);
    return response.data;
  } catch (error) {
    if (axios.isAxiosError(error)) {
      if (error.response) {
        console.error(
          "HTTP response error:",
          error.response.status,
          error.response.data
        );
        throw new Error(`HTTP response error: ${error.response.status}`);
      } else if (error.request) {
        console.error("Server Error");
        throw new Error("Server Error");
      } else {
        console.error("Axios Error:", error.message);
        throw new Error("Axios Error");
      }
    } else {
      console.error("Unknown error", error);
      throw new Error(`Unknown error at ${getAllTodoService.name}`);
    }
  }
};

export const deleteTodoService = async (TodoData: DeleteTodo) => {
  const response = await api.delete(`/todo/${TodoData.userId}`, {
    data: { idTodo: TodoData.todoId },
  });
  return response;
};

export const deleteCompletedService = async (idUser: number) => {
  const response = await api.delete(`/todo/${idUser}/completed`);
  return response;
};

export const updateTodoService = async (TodoData: PutTodo) => {
  const response = await api.put(`/todo/${TodoData.idUser}`, {
    idTodo: TodoData.idTodo,
    idUser: TodoData.idUser,
    estado: TodoData.estado,
  });
  return response;
};
