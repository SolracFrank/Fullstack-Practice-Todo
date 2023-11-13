import { filteredDataSetup } from "../../../interfaces/interface";
import DeleteCompletedButton from "./deleteCompleted";

interface FilterProps {
  setFilteredData: (filteredData: filteredDataSetup) => void;
}
const TodoFilter = ({ setFilteredData }: FilterProps) => {
  return (
    <div
      className="p-4 rounded-b-2xl shadow-lg  
     w-2/3 min-h-4 h-fit overflow-y-scroll overflow-x-hidden no-scrollbar border-t 
    flex justify-evenly
    bg-gray-100 dark:bg-oscure-400 text-black dark:text-oscure-100  shadow-oscure-100 dark:shadow-oscure-400 border-t-teal-400 dark:border-t-calid-200"
    >
      <button
        onClick={() => setFilteredData(filteredDataSetup.All)}
        className="hover:text-oscure-200 active:text-calid-300"
      >
        Show All
      </button>
      <button
        onClick={() => setFilteredData(filteredDataSetup.Active)}
        className="hover:text-oscure-200 active:text-calid-300"
      >
        Show active
      </button>
      <button
        onClick={() => setFilteredData(filteredDataSetup.Completed)}
        className="hover:text-oscure-200 active:text-calid-300"
      >
        Show completed
      </button>
      <DeleteCompletedButton />
    </div>
  );
};
export default TodoFilter;
