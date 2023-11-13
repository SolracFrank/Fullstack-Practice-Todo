import { useRouteError } from "react-router-dom";

interface ErrorData {
  statusText?: string;
  message?: string;
}

export default function ErrorPage() {
  const errorRaw = useRouteError();
  const error: ErrorData | undefined =
    typeof errorRaw === "object" ? (errorRaw as ErrorData) : undefined;
  return (
    <div
      id="error-page"
      className="bg-oscure-300 h-screen flex justify-center items-center"
    >
      <div className="text-2xl text-center text-oscure-100 font-bold">
        <h1>Oops!</h1>
        <p className=" text-lg font-normal">
          Sorry, an unexpected error has occurred.
        </p>
        <p className="font-thin text-3xl">
          {error && <i>{error.statusText || error.message} </i>}
        </p>
      </div>
    </div>
  );
}
