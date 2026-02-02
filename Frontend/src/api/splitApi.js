const BASE_URL = "http://localhost:5107/api/split";

export async function addUser(name) {
  const res = await fetch(`${BASE_URL}/users`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ name })
  });
  return res.json();
}

export async function addExpense(userId, category, amount) {
  const res = await fetch(`${BASE_URL}/expenses`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ userId, category, amount })
  });
  return res.json();
}

export async function calculateTransfers() {
  const res = await fetch(`${BASE_URL}/calculate-transfers`);
  return res.json();
}