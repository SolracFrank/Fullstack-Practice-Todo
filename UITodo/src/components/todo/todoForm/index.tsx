import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { AddTodo, TodoInformation } from "../../../interfaces/interface";
import addTodoSchema from "../../../schemas/addTodoSchema";
import { DateConverter } from "../../../helper/dateConverter";
import { useAuthContext } from "../../../context/useAuthContext";
import { useAddTodo } from "../../../hooks/useTodo";
import toast from "react-hot-toast";
import TextInput from "../../formInputs/addTodoInput/textFormInput";
import SelectableInput from "../../formInputs/addTodoInput/selectableFormInput";
import { QueryKey, useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError } from "axios";

type MutationContext = {
  previousTodos: TodoInformation[];
};

const TodoForm = () => {
  const { todoUser } = useAuthContext();
  const queryKey: QueryKey = ["todos"];

  const { addTodo } = useAddTodo();
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<AddTodo>({
    resolver: zodResolver(addTodoSchema),
  });

  const queryClient = useQueryClient();
  const { mutate } = useMutation(addTodo, {
    onMutate: async (newData) => {
      await queryClient.cancelQueries(queryKey);

      const previousTodos =
        queryClient.getQueryData<TodoInformation[]>(queryKey);

      const tempId = Date.now();

      const optimisticTodo = {
        idTodo: tempId,
        description: newData?.description || "",
        dateLimit: newData?.dateLimit,
        estado: "Activo",
      };
      if (previousTodos)
        queryClient.setQueryData<TodoInformation[]>(queryKey, [
          ...previousTodos,
          optimisticTodo,
        ]);
      return { previousTodos };
    },
    onError: (_error: AxiosError, _variables, context: MutationContext) => {
      queryClient.setQueryData(queryKey, context.previousTodos);
      toast.error("Error al insertar: " + _error.message);
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

  function OnSubmit(data: AddTodo) {
    console.log("Data antes de ser convertida: ", data.dateLimit);

    if (todoUser) {
      const newData = {
        ...data,
        dateLimit: DateConverter(data.dateLimit),
        //dateLimit: data.dateLimit,
        userId: todoUser!,
      };
      console.log("Data después de ser convertida: ", newData.dateLimit);
      mutate(newData);
    }
  }

  return (
    <form
      onSubmit={handleSubmit(OnSubmit)}
      className="p-4 my-5   rounded-2xl shadow-lg w-2/3 h-fit
       bg-gray-100 dark:bg-oscure-400 text-oscure-400 dark:text-oscure-100  shadow-oscure-100 dark:shadow-oscure-400 placeholder:text-oscure-100 dark:placeholder:text-oscure-400 "
    >
      <div className="mb-4">
        <TextInput
          errors={errors}
          fieldName="description"
          placeholder="TO DO description"
          register={register}
        />
      </div>

      <section className="w-full">
        <SelectableInput
          errors={errors}
          register={register}
          type="datetime-local"
        />
      </section>

      <button
        type="submit"
        className="bg-teal-400 hover:bg-blue-950 active:bg-day-400 dark:bg-calid-100 text-oscure-100  dark:hover:bg-calid-300 dark:active:bg-calid-350
        py-2 px-4 rounded-3xl focus:outline-none focus:shadow-outline font-bold w-full"
      >
        Añadir Todo
      </button>
    </form>
  );
};

export default TodoForm;
