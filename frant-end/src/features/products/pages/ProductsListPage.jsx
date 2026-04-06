import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { ProductApi } from "../api/products.api";
import ProductsList from "../components/ProductsList";
import { useAuth } from "../../../auth/keycloak/AuthContext";

export default function ProductsListPage() {
  const { tenantId } = useParams();
  const navigate = useNavigate();
  const { hasRole } = useAuth();

  const isAdmin = hasRole("Admin");

  const [items, setItems] = useState([]);

  async function load() {
    const data = await ProductApi.getAll(tenantId);
    setItems(data);
  }

  useEffect(() => {
    load();
  }, [tenantId]);

  async function handleDelete(id) {
    await ProductApi.remove(tenantId, id);
    await load();
  }

  return (
    <div className="p-6 space-y-4">
      <button
        onClick={() => navigate(`/tenants/${tenantId}/products/form`)}
        className="bg-black text-white px-4 py-2 rounded"
      >
        Ajouter
      </button>

      <ProductsList
        items={items}
        isAdmin={isAdmin}
        onEdit={(p) =>
          navigate(`/tenants/${tenantId}/products/form/${p.id}`)
        }
        onDelete={handleDelete}
      />
    </div>
  );
}