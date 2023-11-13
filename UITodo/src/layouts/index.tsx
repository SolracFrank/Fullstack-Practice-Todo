import { Outlet } from "react-router-dom";
import { NavBar } from "../components/navbar";
export const PrincipalLayout = () => {
  return (
    <div className="flex flex-col h-screen font-raleway">
      <NavBar className="flex-shrink-0"/>
      <div id="main-outlet" className="flex-grow flex">
        <Outlet />
      </div>
    </div>
  );
};
