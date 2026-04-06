import { Package } from "lucide-react";

export default function TransactionsList({ items }) {
  if (!items.length) {
    return (
      <p className="text-center text-[#8a7768] p-6">
        Aucun mouvement trouvé
      </p>
    );
  }

  return (
    <div className="space-y-3">
      {items.map((t) => {
        const isPositive =
          t.movementType === "IN" || t.movementType === "ADJUST";

        return (
          <div
            key={t.id}
            className="bg-white border border-[#e6dcd2] rounded-xl p-4 flex justify-between items-center shadow-sm hover:shadow-md transition"
          >
            {/* LEFT */}
            <div className="flex gap-3 items-center">
              <div className="w-12 h-12 bg-[#f3ede7] rounded-xl flex items-center justify-center">
                <Package className="text-[#8a7768]" />
              </div>

              <div>
                <h3 className="font-semibold text-[#3b2f2f]">
                  {t.productName || "Produit"}
                </h3>

                <p className="text-xs text-[#c96c4b]">
                  {t.movementType}
                </p>
              </div>
            </div>

            {/* RIGHT */}
            <div className="text-right">
              <p
                className={`font-bold ${
                  isPositive ? "text-green-600" : "text-red-500"
                }`}
              >
                {isPositive ? "+" : "-"} {t.quantity} {t.unit || "kg"}
              </p>

              <p className="text-xs text-[#8a7768]">
                {new Date(t.createdAt).toLocaleDateString()}
              </p>
            </div>
          </div>
        );
      })}
    </div>
  );
}