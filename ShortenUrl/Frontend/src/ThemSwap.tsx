import React from "react";
import swapIcon from '/img/theme-swap.png'
export function ThemSwap() {
    const [theme, setTheme] = React.useState('halloween');
    const toggleTheme = () => {
        setTheme(theme === 'halloween' ? 'light' : 'halloween');
    };
    // initially set the theme and "listen" for changes to apply them to the HTML tag
    React.useEffect(() => {
        document.querySelector('html')!.setAttribute('data-theme', theme);
    }, [theme]);
    return (
            <img src={swapIcon} className="w-10" onClick={toggleTheme} alt="swap theme"/>
    );
}