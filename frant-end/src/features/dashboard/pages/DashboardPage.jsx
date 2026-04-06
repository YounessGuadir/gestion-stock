import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { dashboardApi } from "../api/dashboard.api";
import {
  Package,
  Tag,
  DollarSign,
  ShoppingCart,
  TrendingDown,
  AlertTriangle,
} from "lucide-react";
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  ResponsiveContainer,
  Tooltip,
  CartesianGrid,
  Cell,
} from "recharts";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";

function KpiCard({ label, value, icon: Icon, subtitle }) {
  return (
    <Card className="border border-slate-200 bg-white shadow-sm transition-all duration-200 hover:shadow-md">
      <CardContent className="p-5">
        <div className="flex items-start justify-between">
          <div className="space-y-1">
            <p className="text-sm font-medium text-slate-500">{label}</p>
            <h2 className="text-3xl font-semibold tracking-tight text-slate-900">{value}</h2>
            {subtitle ? (
              <p className="text-xs text-slate-400">{subtitle}</p>
            ) : null}
          </div>

          <div className="rounded-lg border border-slate-200 bg-slate-50 p-2.5">
            <Icon className="h-4 w-4 text-slate-500" />
          </div>
        </div>
      </CardContent>
    </Card>
  );
}

export default function DashboardPage() {
  const { tenantId } = useParams();
  const [data, setData] = useState(null);

  useEffect(() => {
    dashboardApi
      .get(tenantId)
      .then((res) => {
        setData(res.data);
      })
      .catch((err) => {
        console.error("Dashboard API error:", err);
      });
  }, [tenantId]);

  const stockNormal = data?.stockNormal || 0;
  const stockFaible = data?.stockLow || 0;
  const rupture = data?.stockOut || 0;
  const totalProducts = data?.totalProducts || 1;

  const stats = [
    {
      label: "Produits en stock",
      value: data?.totalProducts || 0,
      icon: Package,
      subtitle: "Produits actifs",
    },
    {
      label: "Catégories",
      value: data?.totalCategories || 0,
      icon: Tag,
      subtitle: "Catégories disponibles",
    },
    {
      label: "Valeur totale du stock",
      value: `${(data?.totalStockValue ?? 0).toLocaleString("fr-FR")} €`,
      icon: DollarSign,
      subtitle: "Valeur estimée",
    },
    {
      label: "Total des transactions",
      value: data?.totalTransactions || 0,
      icon: ShoppingCart,
      subtitle: "Historique global",
    },
  ];

  const catCounts = data?.topCategories || [];
  const criticalProducts = data?.criticalProducts || [];

  const stockStatuses = [
    {
      label: "Stock normal",
      count: stockNormal,
      color: "bg-blue-600",
    },
    {
      label: "Stock faible (≤ 2)",
      count: stockFaible,
      color: "bg-amber-500",
    },
    {
      label: "Rupture",
      count: rupture,
      color: "bg-red-500",
    },
  ];

  return (
    <div className="space-y-6 p-6 bg-slate-50/60 min-h-screen">
      {/* Header */}
      <div className="space-y-1">
        <h1 className="text-2xl font-semibold tracking-tight text-slate-900">
          Tableau de Bord
        </h1>
        <p className="text-sm text-slate-500">
          Vue d&apos;ensemble de votre inventaire
        </p>
      </div>

      {/* KPI Cards */}
      <div className="grid gap-4 sm:grid-cols-2 xl:grid-cols-4">
        {stats.map((s) => (
          <KpiCard
            key={s.label}
            label={s.label}
            value={s.value}
            icon={s.icon}
            subtitle={s.subtitle}
          />
        ))}
      </div>

      {/* Stock Distribution */}
      <Card className="border border-slate-200 bg-white shadow-sm">
        <CardHeader className="pb-3">
          <CardTitle className="text-sm font-semibold text-slate-900">
            Répartition du stock
          </CardTitle>
        </CardHeader>

        <CardContent className="space-y-4">
          {stockStatuses.map((s) => (
            <div key={s.label} className="flex items-center gap-4">
              <span className="w-40 text-sm text-slate-600">{s.label}</span>

              <div className="flex-1">
                <div className="h-2 overflow-hidden rounded-full bg-slate-100">
                  <div
                    className={`h-full rounded-full ${s.color} transition-all duration-500`}
                    style={{
                      width: `${totalProducts ? (s.count / totalProducts) * 100 : 0}%`,
                    }}
                  />
                </div>
              </div>

              <span className="w-8 text-right text-sm font-semibold text-slate-800">
                {s.count}
              </span>
            </div>
          ))}
        </CardContent>
      </Card>

      {/* Top Categories */}
      <Card className="border border-slate-200 bg-white shadow-sm">
        <CardHeader className="pb-3">
          <CardTitle className="text-sm font-semibold text-slate-900">
            Top catégories
          </CardTitle>
        </CardHeader>

        <CardContent>
          {catCounts.length === 0 ? (
            <div className="flex h-[260px] items-center justify-center text-sm text-slate-500">
              Aucune catégorie
            </div>
          ) : (
            <div className="h-[280px] w-full">
              <ResponsiveContainer width="100%" height="100%">
                <BarChart
                  data={catCounts}
                  margin={{ top: 16, right: 16, left: 0, bottom: 0 }}
                  barSize={34}
                >
                  <CartesianGrid
                    strokeDasharray="3 3"
                    vertical={false}
                    stroke="#e2e8f0"
                  />
                  <XAxis
                    dataKey="name"
                    tick={{ fontSize: 12, fill: "#64748b" }}
                    axisLine={false}
                    tickLine={false}
                  />
                  <YAxis
                    allowDecimals={false}
                    tick={{ fontSize: 12, fill: "#64748b" }}
                    axisLine={false}
                    tickLine={false}
                  />
                  <Tooltip
                    cursor={{ fill: "rgba(59,130,246,0.06)" }}
                    contentStyle={{
                      borderRadius: "10px",
                      border: "1px solid #e2e8f0",
                      backgroundColor: "#ffffff",
                      boxShadow: "0 8px 24px rgba(15,23,42,0.08)",
                      fontSize: "13px",
                    }}
                  />
                  <Bar dataKey="count" radius={[6, 6, 0, 0]}>
                    {catCounts.map((_, i) => (
                      <Cell
                        key={i}
                        fill={i % 2 === 0 ? "#2563eb" : "#5b8def"}
                      />
                    ))}
                  </Bar>
                </BarChart>
              </ResponsiveContainer>
            </div>
          )}
        </CardContent>
      </Card>

      {/* Critical Products */}
      <Card className="border border-slate-200 bg-white shadow-sm">
        <CardHeader className="pb-3">
          <CardTitle className="text-sm font-semibold text-slate-900">
            Produits critiques
          </CardTitle>
        </CardHeader>

        <CardContent>
          {criticalProducts.length === 0 ? (
            <div className="flex flex-col items-center justify-center py-10 text-center">
              <div className="mb-3 rounded-full bg-emerald-50 p-3">
                <Package className="h-5 w-5 text-emerald-600" />
              </div>
              <p className="text-sm font-medium text-slate-900">
                Aucun produit critique
              </p>
              <p className="text-xs text-slate-500">
                Tous vos produits ont un stock suffisant
              </p>
            </div>
          ) : (
            <Table>
              <TableHeader>
                <TableRow className="hover:bg-transparent">
                  <TableHead>#</TableHead>
                  <TableHead>Nom</TableHead>
                  <TableHead>Statut</TableHead>
                  <TableHead className="text-right">Qté</TableHead>
                </TableRow>
              </TableHeader>

              <TableBody>
                {criticalProducts.map((p, i) => {
                  const isOut = p.quantity <= 0;

                  return (
                    <TableRow key={p.id}>
                      <TableCell>{i + 1}</TableCell>
                      <TableCell className="font-medium text-slate-900">
                        {p.name}
                      </TableCell>
                      <TableCell>
                        <div className="flex items-center gap-2">
                          {isOut ? (
                            <>
                              <AlertTriangle className="h-4 w-4 text-red-500" />
                              <span className="text-sm text-red-600">Rupture</span>
                            </>
                          ) : (
                            <>
                              <TrendingDown className="h-4 w-4 text-amber-500" />
                              <span className="text-sm text-amber-600">Stock faible</span>
                            </>
                          )}
                        </div>
                      </TableCell>
                      <TableCell className="text-right font-semibold">
                        {p.quantity}
                      </TableCell>
                    </TableRow>
                  );
                })}
              </TableBody>
            </Table>
          )}
        </CardContent>
      </Card>
    </div>
  );
}