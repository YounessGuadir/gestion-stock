// // import { store } from "@/lib/store";
// import { Package, ArrowDown, ArrowUp, Layers } from "lucide-react";

// export default function Dashboard() {
//   const products = store.getProducts();
//   const categories = store.getCategories();
//   const transactions = store.getTransactions();
//   const totalStock = products.reduce((s, p) => s + p.quantity, 0);
//   const donations = transactions.filter(t => t.type === 'donation');
//   const restocks = transactions.filter(t => t.type === 'restock');

//   const stats = [
//     { label: "Produits", value: products.length, icon: Package, color: "text-primary" },
//     { label: "Catégories", value: categories.length, icon: Layers, color: "text-brown" },
//     { label: "Stock total", value: totalStock, icon: ArrowUp, color: "text-success" },
//     { label: "Donations", value: donations.length, icon: ArrowDown, color: "text-destructive" },
//   ];

//   return (
//     <div>
//       <h1 className="text-2xl font-extrabold text-brown mb-6">Tableau de Bord</h1>
//       <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4 mb-8">
//         {stats.map((s) => (
//           <div key={s.label} className="bg-card rounded-xl border border-border p-5 flex items-center gap-4">
//             <div className="p-3 rounded-lg bg-accent">
//               <s.icon className={`h-6 w-6 ${s.color}`} />
//             </div>
//             <div>
//               <p className="text-2xl font-extrabold text-foreground">{s.value}</p>
//               <p className="text-sm text-muted-foreground">{s.label}</p>
//             </div>
//           </div>
//         ))}
//       </div>

//       <h2 className="text-lg font-bold text-brown mb-3">Produits en stock faible</h2>
//       <div className="bg-card rounded-xl border border-border overflow-hidden">
//         {products.filter(p => p.quantity < 20).length === 0 ? (
//           <p className="p-4 text-muted-foreground">Tous les stocks sont suffisants.</p>
//         ) : (
//           <table className="w-full text-sm">
//             <thead>
//               <tr className="border-b border-border bg-accent/50">
//                 <th className="text-left p-3 font-semibold">Produit</th>
//                 <th className="text-left p-3 font-semibold">Quantité</th>
//                 <th className="text-left p-3 font-semibold">Catégorie</th>
//               </tr>
//             </thead>
//             <tbody>
//               {products.filter(p => p.quantity < 20).map(p => (
//                 <tr key={p.id} className="border-b border-border last:border-0">
//                   <td className="p-3 font-semibold">{p.name}</td>
//                   <td className="p-3 text-destructive font-bold">{p.quantity} {p.unit}</td>
//                   <td className="p-3 text-muted-foreground">{categories.find(c => c.id === p.category)?.name}</td>
//                 </tr>
//               ))}
//             </tbody>
//           </table>
//         )}
//       </div>
//     </div>
//   );
// }
