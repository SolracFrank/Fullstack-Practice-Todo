import { useNavigate } from "react-router-dom";
import { useAuthContext } from "../../context/useAuthContext";
import { useEffect } from "react";

const AuthHome = () => {
  const navigate = useNavigate();
  const { isAuthenticated } = useAuthContext();
  useEffect(() => {
    if (isAuthenticated) {
      navigate("/todo");
      return;
    }
    window.location.reload();
  }, [isAuthenticated, navigate]);
  return (
    <div className="bg-oscure-300 h-full flex justify-center">
      <div className="border border-oscure-400 rounded-2xl shadow-lg shadow-oscure-400 h-1/3 w-2/3 bg-oscure-400 mt-16 space-y-4">
        <h1 className="text-center font-bold text-xl text-oscure-100 mt-8">
          Sigue donde lo dejaste
        </h1>
      </div>
    </div>
  );
};
export default AuthHome;
