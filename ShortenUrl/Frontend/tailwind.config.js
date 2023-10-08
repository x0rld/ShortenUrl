/** @type {import('tailwindcss').Config} */


export default {
  content: ["./src/**/*.{html,js,tsx}"], 
  theme: {
    extend: {},
  },
  daisyui: {
    themes: ["light", "halloween"],
  },
  plugins: [require("daisyui")],
}

