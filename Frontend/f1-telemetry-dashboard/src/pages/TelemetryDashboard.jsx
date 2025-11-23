import { useEffect, useState } from "react"
import { HubConnectionBuilder } from "@microsoft/signalr"
import AllTelemetryDashboard from "../components/AllTelemetryDashboard"
import SectorDashboard from "../components/SectorDashboard"

const HUB_URL =
  import.meta.env.VITE_HUB_URL || "http://localhost:5010/hubs/telemetry"
const SECTORS = [1, 2, 3]

export default function MainOverviewPage() {
  const [status, setStatus] = useState("connecting")
  const [telemetry, setTelemetry] = useState(null)
  const [sectorData, setSectorData] = useState({ 1: null, 2: null, 3: null })

  useEffect(() => {
    let active = true

    const connection = new HubConnectionBuilder()
      .withUrl(HUB_URL)
      .withAutomaticReconnect()
      .build()

    connection.on("telemetry", (payload) => {
      const data = JSON.parse(payload)
      if (!active) return

      setTelemetry(data)
      setSectorData((prev) => ({ ...prev, [data.Sector]: data }))
    })

    const startConnection = async () => {
      if (!active) return
      setStatus("connecting")
      try {
        await connection.start()
        await Promise.all(
          SECTORS.map((s) => connection.invoke("JoinSector", s))
        )
        if (active) setStatus("connected")
      } catch {
        if (!active) return
        setStatus("disconnected")
        setTimeout(startConnection, 3000)
      }
    }

    startConnection()
    return () => {
      active = false
      connection.stop()
    }
  }, [])

  return (
    <div className="min-h-screen bg-gradient-to-br from-[#00594f] via-[#00352f] to-[#00594f] p-10">
      <header className="mb-10">
        <h1 className="text-4xl md:text-5xl font-extrabold tracking-wide text-gray-200">
          F1 Telemetry Dashboard
        </h1>
        <p className="text-[#cedc00] mt-2 text-lg tracking-widest">
          Live Testing Session
        </p>
        <div className="flex items-center gap-2 mt-2 text-sm text-gray-200">
          <span
            className={`inline-block h-3 w-3 rounded-full ${
              status === "connected" ? "bg-[#cedc00]" : "bg-red-400"
            }`}
          />
          <span className="tracking-widest uppercase">{status}</span>
        </div>
      </header>

      <section>
        <AllTelemetryDashboard
          telemetry={telemetry}
          connectionStatus={status}
        />
      </section>

      <section className="mt-6 grid gap-4 md:grid-cols-3">
        {SECTORS.map((sector) => (
          <SectorDashboard
            key={sector}
            sector={sector}
            telemetry={sectorData[sector]}
            isActive={telemetry?.Sector === sector}
          />
        ))}
      </section>
    </div>
  )
}
