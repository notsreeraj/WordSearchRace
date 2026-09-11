import Button from '../components/shared/Button'
import { useState } from 'react'

function GameScreen() {
	const [puzzle, setPuzzle] = useState<string[]>([])

	const handleGetPuzzle = () => {
		const apiUrl = 'http://localhost:5267/puzzle'

		fetch(apiUrl)
			.then((response) => {
				if (!response.ok) {
					throw new Error(`Request failed with status ${response.status}`)
				}

				return response.json()
			})
			.then((result) => {
				console.log('Game data:', result)
				setPuzzle(result as string[])
			})
			.catch((error) => {
				console.error('Failed to fetch game data:', error)
			})
	}

	return (
		<div>
			<div>Game screen</div>
			<Button text="Get puzzle" onClick={handleGetPuzzle} />

			{puzzle.length > 0 && (
				<div className="mt-6 grid grid-cols-20 gap-px border border-slate-700 bg-slate-700">
					{puzzle.flatMap((row, rowIndex) =>
						[...row].map((letter, columnIndex) => (
							<Button
								key={`${rowIndex}-${columnIndex}`}
								text={letter}
								onClick={() => {}}
							/>
						))
					)}
				</div>
			)}
		</div>
	)
}

export default GameScreen
