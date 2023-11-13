import { Outlet, useNavigate } from "react-router-dom";
import { useAuthContext } from "../../context/useAuthContext";
import { useEffect } from "react";

export const ProtectedLayout = () => {
    const { isAuthenticated } = useAuthContext();
    const navigate = useNavigate();
  
    useEffect(() => {
      if(!isAuthenticated)
      {
        navigate("/login", { replace: true });
      }
    }, [isAuthenticated, navigate]);
  return (
    <div id="todo-outlet" className="w-full h-full">
      <Outlet />
    </div>
  );
};
