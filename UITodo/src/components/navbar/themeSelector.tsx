import { IconMoon, IconSun } from "@tabler/icons-react";
import { useEffect, useState } from "react";

const ThemeSelector = () => {
  const initialState = localStorage.getItem("theme") || "light";
  const [theme, setTheme] = useState(initialState);
  useEffect(() => {
    localStorage.setItem("theme", theme);
    document.documentElement.classList.remove("light", "dark");
    document.documentElement.classList.add(`${theme}`);
  }, [theme]);

  function HandleClickToggleTheme() {
    setTheme((prevTheme) => (prevTheme == "light" ? "dark" : "light"));
  }

  return (
    <button onClick={HandleClickToggleTheme}>
      {theme === "light" ? (
        <IconMoon size={32} className="text-black" />
      ) : (
        <IconSun size={32} className="text-oscure-100" />
      )}
    </button>
  );
};
export default ThemeSelector;
