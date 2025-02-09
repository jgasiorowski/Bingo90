package org.example.core.generation

import org.example.core.Ticket

/**
 * This class is responsible for building ticket. It receives numbers via [place] function.
 *
 * After all numbers got distributed [build] function balances ticket's empty cells
 */
internal class TicketBuilder(val index: Int) {
    var numbersInTicketCount = 0
    private val columns = Array(9) { Column(it) }
    private val rows = Array(3) { arrayOfNulls<Int?>(9) }

    fun hasSpace(columnIndex: Int): Boolean {
        val howManyInColumn = columns[columnIndex].size
        return howManyInColumn < 3 && numbersInTicketCount < 15
    }

    /**
     * Places number in matching column. This function is not checking if number can be placed.
     *
     * It is expected that caller will use [hasSpace] function first or has other mechanism to track ticket and
     * column capacity
     */
    fun place(value: Int, columnIndex: Int) {
        numbersInTicketCount++
        columns[columnIndex].add(value)
    }

    fun build(): Ticket {
        this.balanceEmptyCells()
        return Ticket(rows)
    }

    private fun balanceEmptyCells() {
        val rowsEmptyCells = Array(3) { RowEmptyCells(it) }
        fun arrangeColumnWithTwoEmptyCells(column: Column){
            val rowIndicesLeft = mutableListOf(0, 1, 2)
            val ordered = rowsEmptyCells.sortedByDescending { c -> c.count }
            val columnIndex = column.index

            (0..1).map {
                val row = ordered[it]
                rowIndicesLeft.remove(row.index)
                row.count--
            }

            rows[rowIndicesLeft.first()][columnIndex] = column.first()
        }
        fun arrangeColumnWithOneEmptyCell(column: Column){
            val columnIndicesLeft = mutableListOf(0, 1, 2)

            val row = rowsEmptyCells.maxBy { c -> c.count }
            val columnIndex = column.index

            columnIndicesLeft.remove(row.index)
            row.count--

            val ordered = column.sortedBy { it }
            rows[columnIndicesLeft.first()][columnIndex] = ordered[0]
            rows[columnIndicesLeft.last()][columnIndex] = ordered[1]
        }
        fun arrangeColumnWithNoEmptyCells(column: Column){
            val columnIndex = column.index
            val ordered = column.sortedBy { it }

            rows[0][columnIndex] = ordered[0]
            rows[1][columnIndex] = ordered[1]
            rows[2][columnIndex] = ordered[2]
        }

        for (column in columns.sortedBy { c -> c.size }) {
            when (column.size){
                1 -> arrangeColumnWithTwoEmptyCells(column)
                2 -> arrangeColumnWithOneEmptyCell(column)
                3 -> arrangeColumnWithNoEmptyCells(column)
            }
        }
    }

    private class Column(val index: Int) : ArrayList<Int?>(3)
    private class RowEmptyCells(val index: Int){
        var count: Int = 4
    }
}