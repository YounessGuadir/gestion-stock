import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { toast } from "sonner";

import { ProductApi } from "../../products/api/products.api";
import { stockApi } from "../api/stock.api";
import StockForm from "../components/StockForm";

export default function StockFormPage() {
  const { tenantId } = useParams();
  const navigate = useNavigate();

  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    async function loadProducts() {
      try {
        const data = await ProductApi.getAll(tenantId);
        console.log("PRODUCTS RAW =>", data);
        setProducts(data);
      } catch (err) {
        console.error("ERROR PRODUCTS =>", err);
        toast.error("Erreur lors du chargement des produits");
      }
    }

    loadProducts();
  }, [tenantId]);

  async function handleSubmit({ productId, quantity }) {
    try {
      setLoading(true);

      await stockApi.adjust(tenantId, productId, {
        quantity,
        createdBy: "youness",
        reason: "Manual UI adjust",
      });

      toast.success("Stock alimenté avec succès ✅");

      setTimeout(() => {
        navigate(`/tenants/${tenantId}/products`);
      }, 1200);
    } catch (err) {
      console.error("STOCK ERROR =>", err);
      toast.error("Erreur lors de l'alimentation du stock ❌");
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="p-6">
      <div className="max-w-lg mx-auto bg-white p-6 rounded-2xl shadow-sm border">
        <h2 className="text-xl font-semibold mb-6 text-[#d96b43]">
          Alimenter le stock
        </h2>

        <StockForm
          products={products}
          onSubmit={handleSubmit}
          loading={loading}
        />
      </div>
    </div>
  );
}