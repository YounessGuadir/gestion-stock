import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { tenantsApi } from "../api/tenants.api";
import { useAuth } from "../../../auth/keycloak/AuthContext";

export default function TenantsPage() {
  const { token } = useAuth();

  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [err, setErr] = useState(null);

  useEffect(() => {
    async function loadTenants() {
      try {
        setLoading(true);
        setErr(null);

        console.log("TOKEN EXISTS?", !!token);

        const data = await tenantsApi.getAll();
        console.log("TENANTS RESPONSE:", data);

        setItems(Array.isArray(data) ? data : []);
      } catch (e) {
        console.error("TENANTS ERROR:", e);
        setErr(e?.response?.data || e?.message || "Unknown error");
      } finally {
        setLoading(false);
      }
    }

    if (token) {
      loadTenants();
    }
  }, [token]);

  if (loading) {
    return <div className="p-6">Loading tenants...</div>;
  }

  if (err) {
    return (
      <div className="p-6 space-y-2">
        <div className="font-semibold text-red-600">Failed to load tenants</div>
        <pre className="text-xs whitespace-pre-wrap">
          {typeof err === "string" ? err : JSON.stringify(err, null, 2)}
        </pre>
      </div>
    );
  }

  return (
    <div className="p-6 space-y-4">
      <h1 className="text-xl font-semibold">Tenants</h1>

      {!items.length ? (
        <div className="text-sm text-muted-foreground">No tenants found.</div>
      ) : (
        <div className="space-y-3">
          {items.map((t) => (
            <div
              key={t.id}
              className="border rounded-lg p-4 bg-white flex items-start justify-between"
            >
              <div>
                <div className="font-medium">{t.name}</div>
                <div className="text-sm text-muted-foreground">{t.slug}</div>
              </div>

              <div className="flex gap-3">
                <Link
                  to={`/tenants/${t.id}/categories`}
                  className="text-sm text-blue-600 underline"
                >
                  Voir les catégories
                </Link>

                {/* <Link
                  to={`/tenants/${t.id}/products`}
                  className="text-sm text-blue-600 underline"
                >
                  Voir les produits
                </Link> */}
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}