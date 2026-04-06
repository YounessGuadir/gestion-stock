import AppLayout from "../layout/AppLayout";
import ProtectedRoute from "./ProtectedRoute";

import TenantsPage from "../../features/tenants/pages/TenantsPage";
import CategoriesPage from "../../features/categories/pages/CategoriesPage";
import ProductsListPage from "../../features/products/pages/ProductsListPage";
import ProductFormPage from "../../features/products/pages/ProductFormPage";
import Home from "../../pages/Home";


import StockFormPage from "../../features/Stock/pages/StockFormPage";
import DonateStockPage from "../../features/Stock/pages/DonateStockPage";
import TransactionsPage from "../../features/Stock/pages/TransactionsPage";
import DashboardPage from "../../features/dashboard/pages/DashboardPage";


export const routes = [
  {
    element: <ProtectedRoute />,
    children: [
      {
        element: <AppLayout />,
        children: [
          { path: "/", element: <TenantsPage /> },

          { path: "/tenants/:tenantId/products", element: <ProductsListPage /> },

          { path: "/tenants/:tenantId/products/form", element: <ProductFormPage /> },

          { path: "/tenants/:tenantId/products/form/:id", element: <ProductFormPage /> },

          { path: "/tenants/:tenantId/categories", element: <CategoriesPage /> },

          // ✅ STOCK
          { path: "/tenants/:tenantId/products/adjust", element: <StockFormPage /> },


          // ✅ DONATE STOCK (IMPORTANT)
          { path: "/tenants/:tenantId/products/donate", element: <DonateStockPage /> },


          {
            path: "/tenants/:tenantId/transactions",
            element: <TransactionsPage />
          },
          {
            path: "/tenants/:tenantId/dashboard",
            element: <DashboardPage />
          },


          // ❌ مؤقتا نحيدو stock list حتى نصلحو
          // { path: "/tenants/:tenantId/products/:productId/stock", element: <StockListPage /> },

          { path: "/home", element: <Home /> },
        ],
      },
    ],
  },
];