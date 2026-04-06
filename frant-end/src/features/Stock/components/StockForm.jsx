import { useMemo, useState } from "react";
import { Package } from "lucide-react";

export default function StockForm({ products = [], onSubmit, loading }) {
  const [productId, setProductId] = useState("");
  const [quantity, setQuantity] = useState("");

  const selectedProduct = useMemo(() => {
    return products.find((p) => String(p.id) === String(productId));
  }, [products, productId]);

  function handleSubmit(e) {
    e.preventDefault();

    if (!productId) {
      alert("Choisir un produit");
      return;
    }

    if (!quantity || Number(quantity) <= 0) {
      alert("Quantité invalide");
      return;
    }

    onSubmit({
      productId,
      quantity: Number(quantity),
    });
  }

  const productName = selectedProduct?.name ?? selectedProduct?.Name ?? "";
  const productQuantity =
    selectedProduct?.quantity ??
    selectedProduct?.Quantity ??
    selectedProduct?.stockQuantity ??
    selectedProduct?.stock ??
    0;

  const productUnit =
    selectedProduct?.unit ??
    selectedProduct?.Unit ??
    "";

  const rawImage =
    selectedProduct?.imageUrl ??
    selectedProduct?.image ??
    selectedProduct?.ImageUrl ??
    selectedProduct?.Image ??
    selectedProduct?.photo ??
    selectedProduct?.Photo ??
    "";

  const API_BASE = "https://localhost:7132";

  const productImage = rawImage
    ? rawImage.startsWith("http")
      ? rawImage
      : `${API_BASE}${rawImage}`
    : "";

  return (
    <form onSubmit={handleSubmit} className="space-y-5">
      <div>
        <label className="block mb-2 text-sm font-medium">
          Produit
        </label>

        <select
          value={productId}
          onChange={(e) => setProductId(e.target.value)}
          className="w-full px-4 py-3 rounded-xl border border-[#e7d5c4] bg-[#fffdf9] focus:outline-none focus:ring-2 focus:ring-[#d96b43]"
        >
          <option value="">Choisir produit</option>

          {products.map((p) => (
            <option key={p.id} value={p.id}>
              {p.name ?? p.Name}
            </option>
          ))}
        </select>
      </div>

      {selectedProduct && (
        <div className="flex items-center gap-3 rounded-xl border border-[#ead8cb] bg-[#f8f3ef] px-4 py-4">
          {productImage ? (
            <img
              src={productImage}
              alt={productName}
              className="h-14 w-14 rounded-lg object-cover border border-[#e7d5c4]"
              onError={(e) => {
                e.currentTarget.style.display = "none";
              }}
            />
          ) : (
            <div className="flex h-14 w-14 items-center justify-center rounded-lg bg-white border border-[#e7d5c4]">
              <Package className="h-6 w-6 text-[#8a7768]" />
            </div>
          )}

          <div>
            <p className="text-sm font-semibold text-[#2b2b2b]">
              {productName}
            </p>
            <p className="text-xs text-[#8a7768]">
              Stock actuel : {productQuantity} {productUnit}
            </p>
          </div>
        </div>
      )}

      <div>
        <label className="block mb-2 text-sm font-medium">
          Quantité
        </label>

        <input
          type="number"
          min="1"
          value={quantity}
          onChange={(e) => setQuantity(e.target.value)}
          placeholder="Entrer quantité"
          className="w-full px-4 py-3 rounded-xl border border-[#d96b43] bg-[#fffdf9] focus:outline-none focus:ring-2 focus:ring-[#d96b43]"
        />
      </div>

      <button
        type="submit"
        disabled={loading}
        className="w-full px-6 py-3 rounded-xl bg-[#d96b43] text-white font-semibold hover:bg-[#c55c36] transition disabled:opacity-60"
      >
        {loading ? "Chargement..." : "Alimenter le stock"}
      </button>
    </form>
  );
}