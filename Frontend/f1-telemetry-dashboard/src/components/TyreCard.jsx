const TyreCard = ({ position, temp, pressure }) => (
  <div className="rounded-md border border-white/10 bg-white/5 px-3 py-2 text-sm">
    <div className="text-[11px] font-medium text-gray-400 mb-2">{position}</div>

    <div className="flex flex-col gap-1 text-gray-300 text-xs">
      <div className="flex justify-between">
        <span>Temp</span>
        <span>{temp != null ? `${temp.toFixed(1)}°C` : "—"}</span>
      </div>
      <div className="flex justify-between">
        <span>Pressure</span>
        <span>{pressure != null ? `${pressure.toFixed(1)} psi` : "—"}</span>    
      </div>
    </div>
  </div>
)

export default TyreCard
