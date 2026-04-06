import { useEffect, useMemo, useState } from "react";
import { useParams } from "react-router-dom";
import { RotateCcw, Package } from "lucide-react";

import { ProductApi } from "../../products/api/products.api";
import { stockApi } from "../api/stock.api";

const ITEMS_PER_PAGE = 5;

export default function TransactionsPage() {
  const { tenantId } = useParams();

  const [products, setProducts] = useState([]);
  const [transactions, setTransactions] = useState([]);

  const [productFilter, setProductFilter] = useState("all");
  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");
  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(false);

  // =========================
  // LOAD PRODUCTS + ALL MOVEMENTS
  // =========================
  useEffect(() => {
    if (!tenantId) return;
    loadPageData();
  }, [tenantId]);

  async function loadPageData() {
    try {
      setLoading(true);

      const productsData = await ProductApi.getAll(tenantId);
      const safeProducts = Array.isArray(productsData) ? productsData : [];
      setProducts(safeProducts);

      // Charger tous les mouvements de tous les produits
      const results = await Promise.all(
        safeProducts.map(async (p) => {
          try {
            const res = await stockApi.getMovements(tenantId, p.id);
            return Array.isArray(res?.data)
              ? res.data.map((m) => ({
                  ...m,
                  productId: m.productId || p.id,
                }))
              : [];
          } catch (err) {
            console.error(`Erreur mouvements produit ${p.id}`, err);
            return [];
          }
        })
      );

      const allMovements = results.flat();
      setTransactions(allMovements);
    } catch (err) {
      console.error(err);
      setProducts([]);
      setTransactions([]);
    } finally {
      setLoading(false);
    }
  }

  // =========================
  // MAP PRODUCTS
  // =========================
  const productMap = useMemo(() => {
    return Object.fromEntries(products.map((p) => [p.id, p]));
  }, [products]);

  // =========================
  // IMAGE URL
  // =========================
  const fileBaseUrl = useMemo(() => {
    const apiBase =
      import.meta.env.VITE_API_BASE_URL || "https://localhost:7132/api";
    return apiBase.replace(/\/api\/?$/, "");
  }, []);

  function getImageUrl(path) {
    if (!path) return null;
    if (path.startsWith("http")) return path;

    const clean = path.startsWith("/") ? path : `/${path}`;
    return `${fileBaseUrl}${clean}`;
  }

  // =========================
  // HELPERS
  // =========================
  function isOut(type) {
    return type === 2 || type === 3; // OUT / DONATION
  }

  function getCategory(productId) {
    const p = productMap[productId];
    return p?.categoryName || p?.category?.name || "Sans catégorie";
  }

  // =========================
  // NORMALIZE
  // =========================
  const normalized = useMemo(() => {
    return (transactions || []).map((t) => {
      const p = productMap[t.productId];

      return {
        id: t.id,
        productId: t.productId,
        name: p?.name || "Produit inconnu",
        image: p?.imageUrl || p?.image,
        unit: p?.unit || "kg",
        quantity: Number(t.quantity || 0),
        date: t.occurredAt || t.date,
        isOut: isOut(t.type),
      };
    });
  }, [transactions, productMap]);

  // =========================
  // FILTER + SORT DESC
  // =========================
  const filtered = useMemo(() => {
    let list = [...normalized];

    if (productFilter !== "all") {
      list = list.filter((t) => t.productId === productFilter);
    }

    if (startDate) {
      list = list.filter((t) => t.date?.slice(0, 10) >= startDate);
    }

    if (endDate) {
      list = list.filter((t) => t.date?.slice(0, 10) <= endDate);
    }

    // Dernier mouvement en premier
    list.sort((a, b) => new Date(b.date) - new Date(a.date));

    return list;
  }, [normalized, productFilter, startDate, endDate]);

  // =========================
  // PAGINATION
  // =========================
  const totalPages = Math.max(1, Math.ceil(filtered.length / ITEMS_PER_PAGE));

  const paginated = filtered.slice(
    (page - 1) * ITEMS_PER_PAGE,
    page * ITEMS_PER_PAGE
  );

  function resetFilters() {
    setProductFilter("all");
    setStartDate("");
    setEndDate("");
    setPage(1);
  }

  // =========================
  // RESET PAGE ON FILTER CHANGE
  // =========================
  useEffect(() => {
    setPage(1);
  }, [productFilter, startDate, endDate]);

  return (
    <div className="space-y-6">
      {/* HEADER */}
      <div>
        <h1 className="text-2xl font-bold tracking-tight text-[#2f2a25]">
          Transactions
        </h1>
        <p className="text-sm text-[#7b6f63]">
          {filtered.length} transaction{filtered.length > 1 ? "s" : ""}
        </p>
      </div>

      {/* FILTERS */}
      <div className="flex flex-wrap items-center gap-3">
        <select
          value={productFilter}
          onChange={(e) => setProductFilter(e.target.value)}
          className="h-11 min-w-[190px] rounded-xl border border-[#ddd3c8] bg-white px-4 text-sm outline-none transition focus:ring-2 focus:ring-[#d6bfa7]"
        >
          <option value="all">Tous les produits</option>
          {products.map((p) => (
            <option key={p.id} value={p.id}>
              {p.name}
            </option>
          ))}
        </select>

        <input
          type="date"
          value={startDate}
          onChange={(e) => setStartDate(e.target.value)}
          className="h-11 rounded-xl border border-[#ddd3c8] px-4 text-sm outline-none transition focus:ring-2 focus:ring-[#d6bfa7]"
        />

        <input
          type="date"
          value={endDate}
          onChange={(e) => setEndDate(e.target.value)}
          className="h-11 rounded-xl border border-[#ddd3c8]  px-4 text-sm outline-none transition focus:ring-2 focus:ring-[#d6bfa7]"
        />

        <button
          onClick={resetFilters}
          className="h-11 rounded-xl border border-[#ddd3c8] px-4 text-sm font-medium text-[#6c6258] transition hover:bg-[#f8f4ef]"
        >
          Réinitialiser
        </button>
      </div>

      {/* LIST */}
      {loading ? (
        <div className="rounded-2xl border border-[#e8ddd2] p-8 text-center text-[#8a7768]">
          Chargement...
        </div>
      ) : paginated.length === 0 ? (
        <div className="flex flex-col items-center rounded-2xl border border-[#e8ddd2]  py-16 text-center">
          <div className="mb-4 flex h-14 w-14 items-center justify-center rounded-full bg-[#f5efe8]">
            <RotateCcw className="h-5 w-5 text-[#8a7768]" />
          </div>
          <p className="text-sm font-medium text-[#3b2f2f]">
            Aucun mouvement trouvé
          </p>
        </div>
      ) : (
        <div className="space-y-3">
          {paginated.map((t) => (
            <div
              key={t.id}
              className="flex items-center justify-between rounded-2xl border border-[#e6dcd2]  px-5 py-4 shadow-sm transition hover:shadow-md"
            >
              <div className="flex items-center gap-3">
                {t.image ? (
                  <img
                    src={getImageUrl(t.image)}
                    alt={t.name}
                    className="h-12 w-12 rounded-xl object-cover"
                  />
                ) : (
                  <div className="flex h-12 w-12 items-center justify-center rounded-xl bg-[#f4efe9]">
                    <Package className="h-5 w-5 text-[#8a7768]" />
                  </div>
                )}

                <div>
                  <p className="text-base font-semibold text-[#1f1f1f]">
                    {t.name}
                  </p>
                  <span className="inline-flex rounded-full bg-[#eef2ff] px-2.5 py-1 text-xs font-medium text-[#4f46e5]">
                    {getCategory(t.productId)}
                  </span>
                </div>
              </div>

              <div className="text-right">
                <p
                  className={`text-[28px] font-extrabold leading-none ${
                    t.isOut ? "text-red-500" : "text-green-500"
                  }`}
                >
                  {t.isOut ? "-" : "+"}
                  {t.quantity} {t.unit}
                </p>
                <p className="mt-1 text-sm text-[#7b6f63]">
                  {t.date
                    ? new Date(t.date).toLocaleDateString("fr-FR")
                    : "--/--/----"}
                </p>
              </div>
            </div>
          ))}
        </div>
      )}

      {/* PAGINATION */}
      {totalPages > 1 && (
        <div className="flex items-center justify-center gap-3 pt-2">
          <button
            disabled={page <= 1}
            onClick={() => setPage((p) => p - 1)}
            className="rounded-xl bg-[#8d877f] px-4 py-2 text-sm font-semibold text-white transition hover:opacity-90 disabled:cursor-not-allowed disabled:opacity-40"
          >
            «
          </button>

          <span className="text-sm text-[#3b2f2f]">
            Page {page} / {totalPages}
          </span>

          <button
            disabled={page >= totalPages}
            onClick={() => setPage((p) => p + 1)}
            className="rounded-xl bg-[#111111] px-4 py-2 text-sm font-semibold text-white transition hover:opacity-90 disabled:cursor-not-allowed disabled:opacity-40"
          >
            »
          </button>
        </div>
      )}
    </div>
  );
}