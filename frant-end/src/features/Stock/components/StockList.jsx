export default function StockList({ items }) {
  if (!items.length) {
    return (
      <div className="text-sm text-[#8a7768]">
        Aucun mouvement.
      </div>
    );
  }

  return (
    <div className="space-y-3">
      {items.map((m) => (
        <div
          key={m.id}
          className="p-4 rounded-xl border bg-[#fffdf9] flex justify-between"
        >
          <div>
            <div className="font-semibold">
              {m.type === "IN" ? "Entrée" : "Sortie"}
            </div>

            <div className="text-sm text-[#8a7768]">
              {m.reason}
            </div>
          </div>

          <div className="text-right">
            <div className="font-bold">
              {m.quantity}
            </div>

            <div className="text-xs text-[#8a7768]">
              {new Date(m.date).toLocaleDateString()}
            </div>
          </div>
        </div>
      ))}
    </div>
  );
}