import { NavLink } from "react-router-dom";

interface NavProps {
  link?: string;
  navLinkDescription: string;
}
const NavLinks = ({ link="", navLinkDescription }: NavProps) => {
  const linkStyles =
    "text-black dark:text-oscure-100 text-3xl hover:text-day-300 hover:dark:text-gray-300 transition duration-200";
    const fullLink = '/'+link;
  return (
    <NavLink
      to={fullLink}
      className={({ isActive }) => (isActive ? "font-bold" : "")}
    >
      <h3 className={linkStyles}>{navLinkDescription}</h3>
    </NavLink>
  );
};
export default NavLinks;
