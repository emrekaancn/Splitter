import { useState } from "react";

const API_BASE = "http://localhost:5107/api/split";

export default function App() {
  const users = [
    { id: 1, name: "Emre" },
    { id: 2, name: "Akın" },
    { id: 3, name: "Ege" },
    { id: 4, name: "Görkem" },  
  ];

  const [userId, setUserId] = useState(1);    
  const [category, setCategory] = useState("Yemek");
  const [amount, setAmount] = useState("");
  const [message, setMessage] = useState("");
  const [settlements, setSettlements] = useState([]);

  const addExpense = async () => {
    setMessage("");

    if (!amount) {
      setMessage("Miktar gir");
      return;
    }

    const res = await fetch(`${API_BASE}/expenses`, {
  method: "POST",
  headers: { "Content-Type": "application/json" },
  body: JSON.stringify({
    amount: Number(amount),
    category,
    userId: Number(userId),
    }),
  });

    if (res.ok) {
      setMessage("✅ Harcama eklendi");
      setAmount("");
    } else {
      setMessage("❌ Harcama eklenemedi");
    }
  };

  const calculate = async () => {
    const res = await fetch(`${API_BASE}/settlements`);
    if (res.ok) {
      const data = await res.json();
      setSettlements(data);
    }
  };

  return (
    <div
      style={{
        minHeight: "100vh",
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        background: "#111",
        color: "#fff",
      }}
    >
      <div style={{ width: "100%", maxWidth: 400, padding: 20 }}>
        <h1 style={{ textAlign: "center", marginBottom: 20 }}>Split App</h1>

        <label>Kullanıcı</label>
        <select
          value={userId}
          onChange={(e) => setUserId(e.target.value)}
          style={{ width: "100%", padding: 10, marginBottom: 10 }}
        >
          {users.map((u) => (
            <option key={u.id} value={u.id}>
              {u.name}
            </option>
          ))}
        </select>

        <label>Kategori</label>
        <input
          value={category}
          onChange={(e) => setCategory(e.target.value)}
          style={{ width: "100%", padding: 10, marginBottom: 10 }}
        />

        <label>Miktar</label>
        <input
          type="number"
          value={amount}
          onChange={(e) => setAmount(e.target.value)}
          style={{ width: "100%", padding: 10, marginBottom: 10 }}
        />

        <button
          onClick={addExpense}
          style={{
            width: "100%",
            padding: 12,
            marginBottom: 10,
            background: "#22c55e",
            border: "none",
            fontWeight: "bold",
          }}
        >
          Harcama Ekle
        </button>

        <button
          onClick={calculate}
          style={{
            width: "100%",
            padding: 12,
            background: "#3b82f6",
            border: "none",
            fontWeight: "bold",
          }}
        >
          Hesapla
        </button>

        {message && (
          <p style={{ textAlign: "center", marginTop: 10 }}>{message}</p>
        )}

        {settlements.length > 0 && (
          <div style={{ marginTop: 20 }}>
            <h3>Ödemeler</h3>
            {settlements.map((s, i) => (
              <p key={i}>
                {s.name} → {s.amount}₺
              </p>
            ))}
          </div>
        )}
      </div>
    </div>
  );
}