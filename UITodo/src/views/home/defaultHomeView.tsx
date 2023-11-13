import { Link } from "react-router-dom";

const DefaultHome = () => {
  return (
    <div className="bg-gray-50 grid-rows-2 h-full  max-h-full no-scrollbar overflow-hidden">
      <section
        id="anounce"
        className="w-full h-fit py-6 space-y-10 flex flex-col justify-center row-span-1"
      >
        <h1 className="text-center text-4xl">Organiza tu vida</h1>
        <h1 className="text-center text-2xl">
          Lleva un seguimiento a tus actividades
        </h1>
        <Link
          to="/register"
          className="w-2/3 mx-auto font-semibold  text-2xl rounded-full  text-center  p-4 transition duration-200
          bg-calid-400 hover:bg-blue-950 active:bg-day-400 dark:bg-calid-100 text-oscure-100  dark:hover:bg-calid-300 dark:active:bg-calid-350
          "
        >
          Empezar a organizar
        </Link>
        <div className="flex justify-center text-center w-full">
          <Link
            to="/login"
            className="underline cursor-pointer active:text-calid-300 mx-auto"
          >
            ¿Ya tienes una cuenta? Inicia sesión
          </Link>
        </div>
      </section>

      <section
        id="gallery"
        className="w-full h-full flex  justify-center row-span-1 bg-teal-400 dark:bg-black"
      >
        <div className="w-1/3 h-[300px] border my-5  rounded-2xl overflow-hidden border-gray-500">
          <div
            className="p-4 rounded-t-2xl shadow-lg w-full  max-h-full min-h-fit overflow-y-scroll overflow-x-hidden no-scrollbar
            bg-gray-100 dark:bg-oscure-400 text-black dark:text-oscure-100 "
          >
            <ul className="space-y-2 overflow-hidden">
              <li className="p-2 grid grid-cols-3 gap-x-2 rounded-lg hover:bg-oscure-500 transition duration-200 items-center
              bg-white dark:bg-oscure-300">
                <div className="col-span-1">Estudiar JavaScript</div>
                <div className="col-span-1 flex flex-col items-start gap-y-1">
                  <span className="font-bold">LÍMITE</span>
                  06 de Abril del 2023 a las 1:00 pm
                </div>
                <div className="col-span-1 flex justify-between px-10">
                  <div
                    className="px- py-1 w-fit rounded-full cursor-pointer 
                    bg-day-400 active:bg-day-200 dark:bg-calid-400 dark:active:bg-calid-350"
                  >
                    Completado
                  </div>
                </div>
              </li>
              <li className="p-2 grid grid-cols-3 gap-x-2 rounded-lg hover:bg-oscure-500 transition duration-200 items-center
              bg-white dark:bg-oscure-300">
                <div className="col-span-1">Estudiar ASP.NET</div>
                <div className="col-span-1 flex flex-col items-start gap-y-1">
                  <span className="font-bold">LÍMITE</span>
                  06 de Abril del 2023 a las 2:00 pm
                </div>
                <div className="col-span-1 flex justify-between px-10">
                  <div
                    className="px- py-1 w-fit rounded-full cursor-pointer 
                    bg-teal-100 active:bg-white dark:bg-calid-300 dark:active:bg-calid-100"
                  >
                    Activo
                  </div>
                </div>
              </li>
              <li className="p-2 grid grid-cols-3 gap-x-2 rounded-lg hover:bg-oscure-500 transition duration-200 items-center
              bg-white dark:bg-oscure-300">
                <div className="col-span-1">Entrenar en el gym </div>
                <div className="col-span-1 flex flex-col items-start gap-y-1">
                  <span className="font-bold">LÍMITE</span>
                  06 de Abril del 2023 a las 3:00 pm
                </div>
                <div className="col-span-1 flex justify-between px-10">
                  <div
                    className="px- py-1 w-fit rounded-full cursor-pointer 
                    bg-teal-100 active:bg-white dark:bg-calid-300 dark:active:bg-calid-100"
                  >
                    Activo
                  </div>
                </div>
              </li>
              <li className="p-2 grid grid-cols-3 gap-x-2 rounded-lg hover:bg-oscure-500 transition duration-200 items-center
              bg-white dark:bg-oscure-300">
                <div className="col-span-1">Hacer de comer</div>
                <div className="col-span-1 flex flex-col items-start gap-y-1">
                  <span className="font-bold">LÍMITE</span>
                  06 de Abril del 2023 a las 5:00 pm
                </div>
                <div className="col-span-1 flex justify-between px-10">
                  <div
                    className="px- py-1 w-fit rounded-full cursor-pointer 
                    bg-teal-100 active:bg-white dark:bg-calid-300 dark:active:bg-calid-100"
                  >
                    Activo
                  </div>
                </div>
              </li>
              <li className="p-2 grid grid-cols-3 gap-x-2 rounded-lg hover:bg-oscure-500 transition duration-200 items-center
              bg-white dark:bg-oscure-300">
                <div className="col-span-1">Horas libres</div>
                <div className="col-span-1 flex flex-col items-start gap-y-1">
                  <span className="font-bold">LÍMITE</span>
                  06 de Abril del 2023 a las 8:00 pm
                </div>
                <div className="col-span-1 flex justify-between px-10">
                  <div
                    className="px- py-1 w-fit rounded-full cursor-pointer 
                    bg-teal-100 active:bg-white dark:bg-calid-300 dark:active:bg-calid-100"
                  >
                    Activo
                  </div>
                </div>
              </li>
            </ul>
          </div>
        </div>
      </section>
    </div>
  );
};
export default DefaultHome;
