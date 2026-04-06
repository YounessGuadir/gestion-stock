import { Outlet } from "react-router-dom";
import Navbar from "./Navbar";


export default function AppLayout() {
  return (
    <div className="min-h-screen">
      <Navbar />
      <main  className="min-h-screen bg-[#f7f1e7] w-full">
        <Outlet />
      </main>
    </div>
  );
}