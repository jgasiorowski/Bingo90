package org.example.core.generation

class NumberGroupsProvider {
    companion object {
        fun provide(): Array<MutableList<Int>> {
            return arrayOf(
                (1..9).toMutableList(),
                (10..19).toMutableList(),
                (20..29).toMutableList(),
                (30..39).toMutableList(),
                (40..49).toMutableList(),
                (50..59).toMutableList(),
                (60..69).toMutableList(),
                (70..79).toMutableList(),
                (80..90).toMutableList()
            )
        }
    }
}