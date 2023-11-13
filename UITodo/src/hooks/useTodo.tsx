import { useCallback } from "react";
import { AddTodo, DeleteTodo, PutTodo } from "../interfaces/interface";
import { addTodoService, deleteTodoService,deleteCompletedService, updateTodoService } from "../services/todoService";

export const useAddTodo = () => {
  const addTodo = useCallback(async (data: AddTodo) => {
    const response = await addTodoService(data);
    return response;
  }, []);

  return { addTodo };
};

export const useDeleteTodo = () => {
  const deleteTodo = useCallback(async (data: DeleteTodo) => {
    const response = await deleteTodoService(data);
    return response;
  }, []);

  return { deleteTodo };
};

export const useDeleteCompleted = () => {
  const deleteCompleted = useCallback(async (data: number) => {
    const response = await deleteCompletedService(data);
    return response;
  }, []);

  return { deleteCompleted };
};

export const useUpdateTodoState = () => {
  const updateTodo = useCallback(async (data: PutTodo) => {
    const response = await updateTodoService(data);
    return response;
  }, []);

  return { updateTodo };
};