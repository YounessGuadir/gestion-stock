import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { stockApi } from "../api/stock.api";
import StockList from "../components/StockList";

export default function StockListPage() {
  const { tenantId, productId } = useParams();

  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    async function load() {
      try {
        const res = await stockApi.getMovements(tenantId, productId);
        setItems(res.data || []);
      } catch (e) {
        console.error(e);
      } finally {
        setLoading(false);
      }
    }

    load();
  }, [tenantId, productId]);

  if (loading) return <div className="p-6">Loading...</div>;

  return (
    <div className="p-6 bg-[#f7f1e7] min-h-screen space-y-4">

      <h1 className="text-xl font-bold text-[#8b4f2f]">
        Historique du stock
      </h1>

      <StockList items={items} />
    </div>
  );
}