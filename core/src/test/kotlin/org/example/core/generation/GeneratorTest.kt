package org.example.core.generation

import org.example.core.extensions.columns
import org.junit.jupiter.api.Assertions.*
import org.junit.jupiter.api.function.Executable
import kotlin.test.Test
import kotlin.test.fail

class GeneratorTest {
    private val strip = Generator().Generate()

    @Test
    fun `Strip contains 6 tickets`() {
        assertEquals(6, strip.tickets.size)
    }

    @Test
    fun `A bingo ticket consists of 9 columns`() {

        assertAll(
            strip.tickets.map { ticket ->
                Executable { assertEquals(9, ticket.columns.size) }
            }
        )
    }

    @Test
    fun `A bingo ticket consists of 3 rows`() {
        assertAll(
            strip.tickets.map { ticket ->
                Executable { assertEquals(3, ticket.rows.size) }
            }
        )
    }

    @Test
    fun `Each ticket row contains five numbers and four blank spaces`() {
        assertAll(
            strip.tickets.map { ticket ->
                ticket.rows.map { row ->
                    Executable { assertEquals(5, row.filterNotNull().size) }
                }
            }.flatten()
        )
    }

    @Test
    fun `Each ticket column consists of one, two or three numbers and never three blanks`() {
        fail("not implemented")
    }

    @Test
    fun `The first column contains numbers from 1 to 9 (only nine)`() {
        fail("not implemented")
    }

    @Test
    fun `The second column numbers from 10 to 19 (ten), the third, 20 to 29 and so on (until last)`() {
        fail("not implemented")
    }

    @Test
    fun `The last column contains numbers from 80 to 90 (eleven)`() {
        fail("not implemented")
    }

    @Test
    fun `Numbers in the ticket columns are ordered from top to bottom (ASC)`() {
        fail("not implemented")
    }

    @Test
    fun `There can be no duplicate numbers between 1 and 90 in the strip`() {
        fail("not implemented")
    }
}