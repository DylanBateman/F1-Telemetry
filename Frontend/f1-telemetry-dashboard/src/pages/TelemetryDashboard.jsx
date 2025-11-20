export default function MainOverviewPage() {
  return (
    <div className="min-h-screen bg-gradient-to-br from-[#00594f] via-[#00352f] to-[#00594f] p-10">
      <header className="mb-10">
        <h1 className="text-4xl md:text-5xl font-extrabold tracking-wide text-gray-200">
          F1 Telemetry Dashboard
        </h1>
        <p className="text-[#cedc00] mt-2 text-lg tracking-widest">
          Live Testing Session
        </p>
      </header>

      <div className="space-y-12">
        <section>
          <h2 className="text-2xl font-semibold text-gray-200 mb-4 tracking-wider">
            Full Telemetry
          </h2>
          <p className="text-gray-400">Waiting for data...</p>
        </section>

        <section>
          <h2 className="text-2xl font-semibold text-gray-200 mb-4 tracking-wider">
            Sector Performance
          </h2>
          <p className="text-gray-400">Waiting for data...</p>
        </section>
      </div>
    </div>
  )
}
