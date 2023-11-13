import { QueryKey, useQuery } from "@tanstack/react-query";
import { getAllTodoService } from "../../../services/todoService";
import { useAuthContext } from "../../../context/useAuthContext";
import { DateTime } from "luxon";
import {
  DragDropContext,
  Droppable,
  Draggable,
  DropResult,
} from "@hello-pangea/dnd";

import { useEffect, useState } from "react";
import { filteredDataSetup } from "../../../interfaces/interface";

import Todo from "./todo";

interface ListProps {
  filter: string;
}

const TodoList = ({ filter }: ListProps) => {
  const { todoUser } = useAuthContext();
  const queryKey: QueryKey = ["todos"];

  const { data, error, isError, isLoading } = useQuery(
    queryKey,
    () => getAllTodoService(todoUser!),
    {
      enabled: true,
      cacheTime: 10 * 60 * 1000,
      staleTime: 10 * 60 * 1000,
      refetchInterval: 5 * 60 * 1000,
      retry: false,
      refetchOnWindowFocus: false,
      retryDelay: (attempt) => Math.pow(2, attempt) * 1000,
    }
  );

  const [filteredData, setFilteredData] = useState(data);

  useEffect(() => {
    if (filter === filteredDataSetup.All) {
      setFilteredData(data);
      return;
    }
    if (
      (filter === filteredDataSetup.Active ||
        filter === filteredDataSetup.Completed) &&
      data
    ) {
      setFilteredData(data.filter((t) => t.estado === filter));
    }
  }, [filter, data]);

  const handleDragEnd = (result: DropResult) => {
    if (!result.destination) return;
    const startIndex = result.source.index;
    const endIndex = result.destination.index;
    const copyTodos = [...filteredData!];
    const [reorderItem] = copyTodos.splice(startIndex, 1);
    copyTodos.splice(endIndex, 0, reorderItem);
    setFilteredData(copyTodos);
  };

  let errorMessage = "desconocido";

  if (typeof error === "string") {
    errorMessage = error;
  }

  if (isError) {
    return <p>Error.. {errorMessage}</p>;
  }

  if (isLoading) {
    return <p>Loading...</p>;
  }
  return (
    <DragDropContext onDragEnd={handleDragEnd}>
      <Droppable droppableId="todos">
        {(droppableProvider) => (
          <div
            className="p-4  rounded-t-2xl shadow-lg w-2/3 max-h-80 min-h-fit overflow-y-scroll overflow-x-hidden no-scrollbar
             bg-gray-100 dark:bg-oscure-400 text-black dark:text-oscure-100  shadow-oscure-100 dark:shadow-oscure-400 "
            ref={droppableProvider.innerRef}
            {...droppableProvider.droppableProps}
          >
            <ul className="space-y-2">
              {filteredData && filteredData.length > 0 ? (
                filteredData.map((todo, index) => {
                  const sqlDate = todo.dateLimit ?? "";
                  const isoDate = sqlDate.replace(" ", "T") + "Z";
                  const dateTime = DateTime.fromISO(isoDate);

                  return (
                    <Draggable
                      index={index}
                      key={todo.idTodo}
                      draggableId={`${todo.idTodo}`}
                    >
                      {(draggableProvider) => (
                        <Todo
                          key={todo.idTodo}
                          todo={todo}
                          dateTime={dateTime}
                          todoUser={todoUser}
                          ref={draggableProvider.innerRef}
                          {...draggableProvider.draggableProps}
                          {...draggableProvider.dragHandleProps}
                        />
                      )}
                    </Draggable>
                  );
                })
              ) : (
                <div className="font-bold text-center text-oscure-400 dark:text-oscure-100 ">
                  <h1>¡Aún no hay TODOS!</h1>
                </div>
              )}
            </ul>
          </div>
        )}
      </Droppable>
    </DragDropContext>
  );
};
export default TodoList;
