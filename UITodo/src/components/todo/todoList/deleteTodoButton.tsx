import { IconX } from "@tabler/icons-react";
import { DeleteTodo, TodoInformation } from "../../../interfaces/interface";
import { QueryKey, useMutation, useQueryClient } from "@tanstack/react-query";
import { useDeleteTodo } from "../../../hooks/useTodo";
import toast from "react-hot-toast";
import { AxiosError } from "axios";

type MutationContext = {
  previousTodos: TodoInformation[];
};

const DeleteTodoButton = ({ todoId, userId }: DeleteTodo) => {
  const { deleteTodo } = useDeleteTodo();
  const queryKey: QueryKey = ["todos"];
  const queryClient = useQueryClient();

  const mutation = useMutation(deleteTodo, {
    onMutate: async () => {
      await queryClient.cancelQueries(queryKey);

      const previousTodos =
        queryClient.getQueryData<TodoInformation[]>(queryKey);

        if (previousTodos)
        queryClient.setQueryData<TodoInformation[]>(queryKey, previousTodos.filter(todo => todo.idTodo !== todoId));


      return {previousTodos};
    },
    onError: (_error: AxiosError, _variables, context: MutationContext) => 
    {
      queryClient.setQueryData(queryKey, context.previousTodos);
      toast.error("Error al eliminar: " + _error.message);
      if (
        _error.response &&
        _error.response.data &&
        (_error.response.status >= 400 || _error.response.status < 500)
      ) {
        toast.error("Hay un error en la petición ", _error.response.data);
      } else {
        toast.error(`Ocurrió un error desconocido`);
      }

    },
    onSettled: () => {
      queryClient.invalidateQueries(queryKey);
    },
  });

  async function handleClick() {
    await mutation.mutate({ userId, todoId });
  }

  return (
    <IconX
      className="ml-2 bg-calid-300 hover:bg-calid-200 text-white py-1 px-2 rounded-full transition duration-200"
      size={36}
      onClick={handleClick}
    />
  );
};

export default DeleteTodoButton;
