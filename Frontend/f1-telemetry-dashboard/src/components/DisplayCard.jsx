const DisplayCard = ({ label, value, unit }) => (
  <div className="flex flex-col rounded-md border border-white/10 bg-white/5 px-2 py-2 text-sm text-gray-100">
    <span className="text-xs text-gray-400 mt-1 mb-1">{label}</span>
    <span className="text-lg font-semibold text-gray-50 leading-tight">
      {value}
      {unit && <span className="ml-1 text-xs text-gray-300">{unit}</span>}
    </span>
  </div>
)

export default DisplayCard
