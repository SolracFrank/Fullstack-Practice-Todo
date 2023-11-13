import { useNavigate } from "react-router-dom";
import RegisterForm from "../../components/registerForm";
import { useAuthContext } from "../../context/useAuthContext";
import { useEffect } from "react";

export const Register = () => {
  const { isAuthenticated } = useAuthContext();
  const navigate = useNavigate();

  useEffect(() => {
    if (isAuthenticated) {
      navigate("/", { replace: true });
    }
  }, [isAuthenticated, navigate]);
  return (
    <div className="bg-day-100 dark:bg-oscure-300 w-full flex justify-center py-10">
      <RegisterForm />
    </div>
  );
};
