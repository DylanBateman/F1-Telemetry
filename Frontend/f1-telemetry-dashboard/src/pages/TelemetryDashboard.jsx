import { useEffect, useState } from "react"
import { HubConnectionBuilder } from "@microsoft/signalr"
import AllTelemetryDashboard from "../components/AllTelemetryDashboard"

const HUB_URL = "http://localhost:5171/hubs/telemetry"

export default function MainOverviewPage() {
  const [status, setStatus] = useState("connecting")
  const [telemetry, setTelemetry] = useState(null)

  useEffect(() => {
    let active = true

    const connection = new HubConnectionBuilder()
      .withUrl(HUB_URL)
      .withAutomaticReconnect()
      .build()

    const startConnection = async () => {
      if (!active) return
      setStatus("connecting")
      try {
        await connection.start()
        if (active) setStatus("connected")
      } catch (e) {
        if (!active) return
        setStatus("disconnected")
        setTimeout(startConnection, 3000)
      }
    }

    connection.on("telemetry", (payload) => setTelemetry(JSON.parse(payload)))

    connection.onreconnecting(() => active && setStatus("reconnecting"))
    connection.onreconnected(() => active && setStatus("connected"))

    connection.onclose(() => {
      if (!active) return
      setStatus("disconnected")
      startConnection()
    })

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
    </div>
  )
}
