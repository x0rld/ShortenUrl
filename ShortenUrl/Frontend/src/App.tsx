import {Create} from "./Create.tsx";
import {ThemSwap} from "./ThemSwap.tsx";

function App() {

  return (
    <>
        <ThemSwap></ThemSwap>
        <p className="text-xl flex justify-center text-center">Just a website to create short url using react and tailwind </p>
      <Create></Create>
    </>
  )
}

export default App
