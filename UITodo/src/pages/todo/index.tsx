import { useState } from "react";
import TodoFilter from "../../components/todo/todoFilter";
import TodoForm from "../../components/todo/todoForm";
import TodoList from "../../components/todo/todoList";
import { filteredDataSetup } from "../../interfaces/interface";

const TodoPage = () => {

  const [filteredData, setFilteredData]= useState<filteredDataSetup>(filteredDataSetup.All);

  function SetFilter (filter:filteredDataSetup)
  {
    setFilteredData(()=>{return filter});
  }
  return (
    <div className="bg-oscure-100 dark:bg-oscure-200 w-full flex flex-col h-full justify-start items-center">
        <TodoForm/>
        <TodoList filter={filteredData}/>
        <TodoFilter setFilteredData={SetFilter} />
    </div>
  );
};
export default TodoPage;
