import { Link, useLocation, useParams } from "react-router-dom";
import { useAuth } from "../../auth/keycloak/AuthContext";

import {
  LayoutDashboard,
  Package,
  PlusCircle,
  List,
  Heart,
  ArrowLeftRight,
  PackagePlus,
  LogOut,
  Box
} from "lucide-react";

export default function Navbar() {
  const location = useLocation();
  const { tenantId } = useParams(); // ✅ هنا خاصها تكون
  const { username, roles, logout } = useAuth();

  const navItems = [
     {
    to: `/tenants/${tenantId}/dashboard`, // ✅ FIX
    label: "Tableau de Bord",
    icon: LayoutDashboard
  },
    { to: `/tenants/${tenantId}/products`, label: "Produits", icon: Package },
    { to: `/tenants/${tenantId}/products/form`, label: "Nouveau produit", icon: PlusCircle },
    { to: `/tenants/${tenantId}/categories`, label: "Catégories", icon: List },
    // 🔥 STOCK
    {
      to: `/tenants/${tenantId}/transactions`,
      label: "Transactions",
      icon: ArrowLeftRight
    },

    { to: `/tenants/${tenantId}/products/adjust`, label: "Alimenter le stock", icon: PackagePlus },
    { to: `/tenants/${tenantId}/products/donate`, label: "Donner", icon: Heart },
  ];

  return (
    <header className="border-b border-[#e7d5c4] bg-[#fffdf9]">
      <div className="w-full px-6 py-3 flex items-center justify-between">

        {/* LEFT */}
        <div className="flex items-center gap-6 overflow-x-auto">

          {/* Logo */}
          <div className="flex items-center gap-2">
            <Box className="h-7 w-7 text-[#d96b43]" />
            <span className="text-xl font-extrabold text-[#8b4f2f]">
              AssoStock
            </span>
          </div>

          {/* Navigation */}
          <nav className="flex items-center gap-2">
            {navItems.map((item) => {
              const active = location.pathname === item.to;

              return (
                <Link
                  key={item.to}
                  to={item.to}
                  className={`flex items-center gap-2 px-4 py-2 rounded-xl text-sm font-semibold transition-all whitespace-nowrap
                    ${active
                      ? "bg-[#f3e2cf] text-[#d96b43]"
                      : "text-[#8a7768] hover:bg-[#f3e2cf] hover:text-[#d96b43]"
                    }`}
                >
                  <item.icon className="h-4 w-4" />
                  {item.label}
                </Link>
              );
            })}
          </nav>
        </div>

        {/* RIGHT */}
        <div className="flex items-center gap-4">

          <div className="text-sm text-[#8a7768]">
            {username ? (
              <span>
                {username}
                {roles?.length
                  ? ` • ${roles.includes("Admin") ? "Admin" : "User"}`
                  : ""}
              </span>
            ) : "..."}
          </div>

          <button
            onClick={logout}
            className="flex items-center gap-2 px-4 py-2 rounded-xl border border-[#e7d5c4] text-[#8b4f2f] hover:bg-[#f3e2cf] transition"
          >
            <LogOut className="h-4 w-4" />
            Logout
          </button>
        </div>
      </div>
    </header>
  );
}