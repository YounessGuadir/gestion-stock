// import { Routes, Route } from "react-router-dom";
// import ProtectedRoute from "./app/router/ProtectedRoute";
// import AppLayout from "./app/layout/AppLayout";


// import TenantsPage from "./features/tenants/pages/TenantsPage";
// import CategoriesPage from "./features/categories/pages/CategoriesPage";
// import ProductsListPage from "./features/products/pages/ProductsListPage";
// import ProductFormPage from "./features/products/pages/ProductFormPage";
import AppRouter from "./app/router";
import { Toaster } from "sonner";



function NotFound() {
  return (
    <div className="p-6">
      <h2 className="text-lg font-semibold">404 - Route not found</h2>
      <p className="text-sm text-muted-foreground">Try /</p>
    </div>
  );
}

export default function App() {
  return (
    <>
      <AppRouter />
      <Toaster richColors position="bottom-right" />
    </>

    // <Routes>
    //   {/* كلشي محمي */}
    //   <Route element={<ProtectedRoute />}>
    //    <Route element={<AppLayout />}>
    //   <Route path="/" element={<TenantsPage />} />
    //   <Route path="/tenants/:tenantId/categories" element={<CategoriesPage />} />
    //   <Route path="/tenants/:tenantId/products" element={<ProductsListPage />} />
    //   <Route path="/tenants/:tenantId/products/form" element={<ProductFormPage />} />
    // </Route>
    //   </Route>

    //   <Route path="*" element={<NotFound />} />
    // </Routes>
  );
}