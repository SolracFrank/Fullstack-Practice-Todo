import { useAuthContext } from "../../context/useAuthContext";
import NavLinks from "./NavLinks";
import ThemeSelector from "./themeSelector";

interface NavBarProps {
  className?: string;
}

export const NavBar: React.FC<NavBarProps> = ({ className }: NavBarProps) => {
  const { isAuthenticated, userName, logout } = useAuthContext();
  return (
    <nav className={`bg-teal-400 dark:bg-oscure-400  p-4 ${className}`}>
      <ul className="flex justify-between px-4">
        <div className="flex space-x-3">
          <NavLinks navLinkDescription="Home" />
          <ThemeSelector />
        </div>

        {isAuthenticated ? (
          <div className="flex gap-x-4 justify-between">
            <li>
              <span className="text-3xl text-black dark:text-oscure-100 ">{userName}</span>
            </li>

            <NavLinks link="todo" navLinkDescription="Mis TODO" />

            <button onClick={logout}>
              <h3 className="text-black dark:text-oscure-100 text-3xl hover:text-gray-300 transition duration-200">
                LOG OUT
              </h3>
            </button>
          </div>
        ) : (
          <div className="flex gap-x-4 justify-between">
            <NavLinks link="login" navLinkDescription="Login" />
            <NavLinks link="register" navLinkDescription="Register" />
          </div>
        )}
      </ul>
    </nav>
  );
};
