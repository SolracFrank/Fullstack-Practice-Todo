import { useEffect } from "react";
import LoginForm from "../../components/loginForm";
import { useAuthContext } from "../../context/useAuthContext";
import { useNavigate } from "react-router-dom";

const Login = () => {
  const { isAuthenticated } = useAuthContext();
  const navigate = useNavigate();

  useEffect(() => {
    if(isAuthenticated)
    {
      navigate("/", { replace: true });
    }
  }, [isAuthenticated, navigate]);
  return (
    <div className="bg-day-100 dark:bg-oscure-300 w-full flex justify-center py-10">
      <LoginForm />
    </div>
  );
};

export default Login;
